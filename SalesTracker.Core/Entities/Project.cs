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
        public  Category Category { get; set; } = null!;

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

        [Column(TypeName = "decimal(18,2)")]
        public decimal ManualMargin { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Hours { get; set; }

        // Financial - Actual (Cafca/Nacalculatie)
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CafcaMargin { get; set; }

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

        // Audit
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
