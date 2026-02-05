using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalesTracker.Core.Data;
using SalesTracker.Core.Entities;
using SalesTracker.Core.Interfaces;
using SalesTracker.Core.Services;

var builder = WebApplication.CreateBuilder(args);

// Debug: Check environment variables at startup
Console.WriteLine("=== ENVIRONMENT CHECK ===");
Console.WriteLine($"ASPNETCORE_ENVIRONMENT: {builder.Environment.EnvironmentName}");
Console.WriteLine($"DATABASE_URL exists: {!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("DATABASE_URL"))}");

var dbUrlCheck = Environment.GetEnvironmentVariable("DATABASE_URL");
if (!string.IsNullOrEmpty(dbUrlCheck))
{
    Console.WriteLine($"DATABASE_URL format: {(dbUrlCheck.StartsWith("postgresql://") || dbUrlCheck.StartsWith("postgres://") ? "PostgreSQL URL" : "Connection String")}");
    Console.WriteLine($"DATABASE_URL length: {dbUrlCheck.Length} characters");
}
Console.WriteLine("========================");

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure database connection
string? connectionString = null;

// In production, prioritize DATABASE_URL environment variable
if (builder.Environment.IsProduction())
{
    var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

    if (!string.IsNullOrEmpty(databaseUrl))
    {
        Console.WriteLine("Using DATABASE_URL from environment");

        // Convert DATABASE_URL to connection string format if needed
        if (databaseUrl.StartsWith("postgres://") || databaseUrl.StartsWith("postgresql://"))
        {
            try
            {
                // Parse the URL format
                var uri = new Uri(databaseUrl);
                var userInfo = uri.UserInfo.Split(':');
                var username = userInfo[0];
                var password = userInfo.Length > 1 ? userInfo[1] : "";

                connectionString = $"Host={uri.Host};Port={uri.Port};Database={uri.AbsolutePath.TrimStart('/')};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true";

                Console.WriteLine($"✓ Parsed DATABASE_URL successfully");
                Console.WriteLine($"Host: {uri.Host}");
                Console.WriteLine($"Port: {uri.Port}");
                Console.WriteLine($"Database: {uri.AbsolutePath.TrimStart('/')}");
                Console.WriteLine($"Username: {username}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Failed to parse DATABASE_URL: {ex.Message}");
            }
        }
        else
        {
            // Use as-is if already in connection string format
            connectionString = databaseUrl;
            Console.WriteLine("Using DATABASE_URL as connection string format");
        }
    }
    else
    {
        Console.WriteLine("⚠️ DATABASE_URL environment variable not found");
    }
}

// Fallback to appsettings.json for development
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    Console.WriteLine($"Using connection string from appsettings: {!string.IsNullOrEmpty(connectionString)}");
}

// Log connection string info (masked for security)
Console.WriteLine($"Connection string configured: {!string.IsNullOrEmpty(connectionString)}");
if (!string.IsNullOrEmpty(connectionString))
{
    var maskedConnection = connectionString.Length > 50
        ? connectionString.Substring(0, 50) + "..."
        : connectionString;
    Console.WriteLine($"Connection format: {maskedConnection}");
}

if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("❌ CRITICAL: No connection string found!");
    throw new InvalidOperationException("Database connection string is not configured. Set DATABASE_URL environment variable or configure DefaultConnection in appsettings.json");
}

// Configure Npgsql to handle DateTime as UTC
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorCodesToAdd: null);
        npgsqlOptions.CommandTimeout(60);
    });

    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

// Configure Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Configure cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(24);
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
});

// Add health checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>();

builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ILeadChannelService, LeadChannelService>();
builder.Services.AddScoped<ICategoryTargetService, CategoryTargetService>();
builder.Services.AddScoped<ICsvService, CsvService>();

// Add CORS for development
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
    });
}

var app = builder.Build();

// Auto-migrate database and seed data on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var appLogger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        appLogger.LogInformation("Starting database migration...");
        var db = services.GetRequiredService<ApplicationDbContext>();

        // Test connection with detailed error logging
        appLogger.LogInformation("Testing database connection...");
        appLogger.LogInformation($"Provider: {db.Database.ProviderName}");

        var canConnect = await db.Database.CanConnectAsync();

        if (!canConnect)
        {
            appLogger.LogError("❌ Cannot connect to database - CanConnectAsync returned false");
            throw new Exception("Database connection test failed");
        }

        appLogger.LogInformation("✓ Database connection successful");

        // Apply migrations
        appLogger.LogInformation("Applying migrations...");

        var pendingMigrations = await db.Database.GetPendingMigrationsAsync();
        appLogger.LogInformation($"Pending migrations: {pendingMigrations.Count()}");

        if (pendingMigrations.Any())
        {
            appLogger.LogInformation($"Migrations to apply: {string.Join(", ", pendingMigrations)}");
        }

        await db.Database.MigrateAsync();
        appLogger.LogInformation("✓ Database migrations applied successfully");

        // Seed admin user and roles
        appLogger.LogInformation("Seeding admin user and roles...");
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await DataSeeder.SeedAdminUser(userManager, roleManager);
        appLogger.LogInformation("✓ Admin user and roles seeded successfully");
    }
    catch (Exception ex)
    {
        appLogger.LogError("❌ Database operation failed");
        appLogger.LogError($"Error Type: {ex.GetType().Name}");
        appLogger.LogError($"Error Message: {ex.Message}");

        if (ex.InnerException != null)
        {
            appLogger.LogError($"Inner Exception Type: {ex.InnerException.GetType().Name}");
            appLogger.LogError($"Inner Exception Message: {ex.InnerException.Message}");
        }

        appLogger.LogError($"Stack Trace: {ex.StackTrace}");

        // In production, log but don't crash - let health checks handle it
        if (app.Environment.IsDevelopment())
        {
            throw;
        }
        else
        {
            appLogger.LogWarning("⚠️ Application will start without database connection");
            appLogger.LogWarning("⚠️ Database operations will fail until connection is fixed");
        }
    }
}

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseCors();
}

app.UseStaticFiles();
app.UseRouting();

// Authentication & Authorization (order matters!)
app.UseAuthentication();
app.UseAuthorization();

// Add health check endpoint
app.MapHealthChecks("/health");

// Add detailed health endpoint for debugging
app.MapGet("/health/detailed", async (ApplicationDbContext context) =>
{
    try
    {
        var canConnect = await context.Database.CanConnectAsync();
        var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
        var appliedMigrations = await context.Database.GetAppliedMigrationsAsync();

        return Results.Ok(new
        {
            status = canConnect ? "healthy" : "unhealthy",
            database = new
            {
                connected = canConnect,
                pendingMigrations = pendingMigrations.Count(),
                appliedMigrations = appliedMigrations.Count(),
                provider = context.Database.ProviderName
            },
            timestamp = DateTime.UtcNow
        });
    }
    catch (Exception ex)
    {
        return Results.Ok(new
        {
            status = "unhealthy",
            error = ex.Message,
            innerError = ex.InnerException?.Message,
            timestamp = DateTime.UtcNow
        });
    }
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
