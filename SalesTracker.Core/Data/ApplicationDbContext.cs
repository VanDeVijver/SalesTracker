using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SalesTracker.Core.Entities;

namespace SalesTracker.Core.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<LeadChannel> LeadChannels { get; set; }
        public DbSet<CategoryTarget> CategoryTargets { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Project entity
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Customer)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.LostReason)
                    .HasMaxLength(100);

                entity.Property(e => e.Notes)
                    .HasMaxLength(2000);

                entity.Property(e => e.ProjectLog)
            .HasColumnType("text")
            .IsRequired(false);

                // Decimal properties
                entity.Property(e => e.Amount)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Purchase)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.ManualMarginPercentage)
                    .HasColumnType("decimal(5,2)");

                entity.Property(e => e.Hours)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.CafcaMarginPercentage)
                    .HasColumnType("decimal(5,2)");

                entity.Property(e => e.CafcaHours)
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.FinalInvoiceAmount)
                    .HasColumnType("decimal(18,2)");

                // Relationships - Use the navigation properties from Category and LeadChannel
                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Projects)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.LeadChannel)
                    .WithMany(l => l.Projects)
                    .HasForeignKey(e => e.LeadChannelId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Indexes
                entity.HasIndex(e => e.Date);
                entity.HasIndex(e => e.Status);
                entity.HasIndex(e => e.CategoryId);
                entity.HasIndex(e => e.LeadChannelId);
            });

            // Configure Category entity
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(e => e.Name)
                    .IsUnique();
            });

            // Configure LeadChannel entity
            modelBuilder.Entity<LeadChannel>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasIndex(e => e.Name)
                    .IsUnique();
            });

            // Configure CategoryTarget entity
            modelBuilder.Entity<CategoryTarget>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.TargetAmount)
                    .HasColumnType("decimal(18,2)");

                // Relationship using navigation property from Category
                entity.HasOne(e => e.Category)
                    .WithMany(c => c.Targets)
                    .HasForeignKey(e => e.CategoryId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Unique constraint - one target per category per year
                entity.HasIndex(e => new { e.CategoryId, e.Year })
                    .IsUnique();
            });

            // Configure SystemSetting entity
            modelBuilder.Entity<SystemSetting>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Key)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Value)
                    .IsRequired();

                entity.HasIndex(e => e.Key)
                    .IsUnique();
            });

            // Seed initial data
            DataSeeder.Seed(modelBuilder);
        }
    }
}
