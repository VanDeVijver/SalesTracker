using Microsoft.EntityFrameworkCore;
using SalesTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesTracker.Core.Data
{
    public static class DataSeeder
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            seedCategories(modelBuilder);
            seddLeadChannels(modelBuilder);
            seedSystemSettings(modelBuilder);
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

        private static void seddLeadChannels(ModelBuilder modelBuilder)
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

    }
}
