using SalesTracker.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesTracker.Core.Entities
{
    public class Project : BaseEntity
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(200)]
        public string Customer { get; set; } = string.Empty;

        public ClientType ClientType { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; } = null!;

        [Required]
        public int LeadChannelId { get; set; }

        [ForeignKey(nameof(LeadChannelId))]
        public LeadChannel LeadChannel { get; set; } = null!;

        public ProjectStatus Status { get; set; }

        // Financial - Quote (Offerte)
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Purchase { get; set; }

        // Changed to percentage (0-100)
        [Column(TypeName = "decimal(5,2)")]
        public decimal ManualMarginPercentage { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Hours { get; set; }

        // Financial - Actual (Cafca/Nacalculatie)
        // Changed to percentage (0-100)
        [Column(TypeName = "decimal(5,2)")]
        public decimal? CafcaMarginPercentage { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? CafcaHours { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? FinalInvoiceAmount { get; set; }

        // Status-specific fields
        public DateTime? EndDate { get; set; }

        [MaxLength(100)]
        public string? LostReason { get; set; }

        [MaxLength(2000)]
        
        public string? Notes { get; set; }
        // ===== NEW: Project Checklist =====
        public bool CheckCafca { get; set; }
        public bool CheckFolder { get; set; }
        public bool CheckMaterial { get; set; }
        public bool CheckPlanning { get; set; }

        // ===== NEW: Project Log (stored as JSON) =====
        [Column(TypeName = "text")]
        public string? ProjectLog { get; set; }
        // Audit
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Calculated properties
        [NotMapped]
        public decimal ManualMarginAmount => Amount * (ManualMarginPercentage / 100);

        [NotMapped]
        public decimal? CafcaMarginAmount => FinalInvoiceAmount.HasValue && CafcaMarginPercentage.HasValue
            ? FinalInvoiceAmount.Value * (CafcaMarginPercentage.Value / 100)
            : null;

        [NotMapped]
        public List<ProjectLogEntry> LogEntries
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ProjectLog))
                    return new List<ProjectLogEntry>();

                try
                {
                    return System.Text.Json.JsonSerializer.Deserialize<List<ProjectLogEntry>>(ProjectLog)
                           ?? new List<ProjectLogEntry>();
                }
                catch
                {
                    return new List<ProjectLogEntry>();
                }
            }
            set
            {
                ProjectLog = System.Text.Json.JsonSerializer.Serialize(value);
            }
        }
    }
}

