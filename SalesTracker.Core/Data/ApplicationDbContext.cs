using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SalesTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesTracker.Core.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<LeadChannel> LeadChannels { get; set; } = null!;
        public DbSet<CategoryTarget> CategoryTargets { get; set; } = null!;
        public DbSet<SystemSetting> SystemSettings { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //user seeding:
            var adminRoleId = Guid.NewGuid().ToString();
            var managerRoleId = Guid.NewGuid().ToString();
            var userRoleId = Guid.NewGuid().ToString();

            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole
                {
                    Id = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = managerRoleId,
                    Name = "Manager",
                    NormalizedName = "MANAGER"
                },
                new IdentityRole
                {
                    Id = userRoleId,
                    Name = "User",
                    NormalizedName = "USER"
                }
            );

            // Seed Admin User
            var adminUserId = Guid.NewGuid().ToString();
            var hasher = new PasswordHasher<ApplicationUser>();

            var adminUser = new ApplicationUser
            {
                Id = adminUserId,
                UserName = "admin@salestracker.com",
                NormalizedUserName = "ADMIN@SALESTRACKER.COM",
                Email = "admin@salestracker.com",
                NormalizedEmail = "ADMIN@SALESTRACKER.COM",
                EmailConfirmed = true,
                FirstName = "System",
                LastName = "Administrator",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin@123");

            modelBuilder.Entity<ApplicationUser>().HasData(adminUser);

            // Assign Admin role to Admin user
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    RoleId = adminRoleId,
                    UserId = adminUserId
                }
            );

            // Configure indexes
            modelBuilder.Entity<Project>()
                .HasIndex(p => new { p.Date, p.Status })
                .HasDatabaseName("IX_Projects_Date_Status");

            modelBuilder.Entity<Project>()
                .HasIndex(p => p.CategoryId)
                .HasDatabaseName("IX_Projects_CategoryId");

            modelBuilder.Entity<Project>()
                .HasIndex(p => p.LeadChannelId)
                .HasDatabaseName("IX_Projects_LeadChannelId");

            modelBuilder.Entity<CategoryTarget>()
                .HasIndex(ct => new { ct.Year, ct.CategoryId })
                .IsUnique()
                .HasDatabaseName("IX_CategoryTargets_Year_CategoryId");

            modelBuilder.Entity<SystemSetting>()
                .HasIndex(s => s.Key)
                .IsUnique()
                .HasDatabaseName("IX_SystemSettings_Key");

            DataSeeder.Seed(modelBuilder);
        }
    }
}
