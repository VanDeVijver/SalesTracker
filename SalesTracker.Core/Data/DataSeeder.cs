using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SalesTracker.Core.Entities;
using System;

namespace SalesTracker.Core.Data
{
    public static class DataSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            seedCategories(modelBuilder);
            seedLeadChannels(modelBuilder);
            seedSystemSettings(modelBuilder);
            seedRoles(modelBuilder);
        }

        private static void seedCategories(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(
               new Category { Id = 1, Name = "PV-BAT B2C", IsActive = true },
               new Category { Id = 2, Name = "PV-BAT B2B", IsActive = true },
               new Category { Id = 3, Name = "WeVolt", IsActive = true },
               new Category { Id = 4, Name = "Lightweight PV", IsActive = true },
               new Category { Id = 5, Name = "BESS", IsActive = true },
               new Category { Id = 6, Name = "Charge", IsActive = true },
               new Category { Id = 7, Name = "ALSB", IsActive = true }
           );
        }

        private static void seedLeadChannels(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LeadChannel>().HasData(
                new LeadChannel { Id = 1, Name = "Eigen leads", IsActive = true, CreatedAt = DateTime.UtcNow },
                new LeadChannel { Id = 2, Name = "Davy", IsActive = true, CreatedAt = DateTime.UtcNow },
                new LeadChannel { Id = 3, Name = "SMA", IsActive = true, CreatedAt = DateTime.UtcNow },
                new LeadChannel { Id = 4, Name = "SolarWatt", IsActive = true, CreatedAt = DateTime.UtcNow },
                new LeadChannel { Id = 5, Name = "Wienerberger", IsActive = true, CreatedAt = DateTime.UtcNow }
            );
        }

        private static void seedSystemSettings(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SystemSetting>().HasData(
                new SystemSetting { Id = 1, Key = "HourlyRate", Value = "50", UpdatedAt = DateTime.UtcNow });
        }

        private static void seedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = "1",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new IdentityRole
                {
                    Id = "2",
                    Name = "Manager",
                    NormalizedName = "MANAGER",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                },
                new IdentityRole
                {
                    Id = "3",
                    Name = "User",
                    NormalizedName = "USER",
                    ConcurrencyStamp = Guid.NewGuid().ToString()
                }
            );
        }

        public static async Task SeedAdminUser(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            var roles = new[] { "Admin", "Manager", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminEmail = "admin@salestracker.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    LastName = "User"
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}
