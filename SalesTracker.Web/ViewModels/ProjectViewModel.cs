using System.ComponentModel.DataAnnotations;
using SalesTracker.Core.Models;

namespace SalesTracker.Web.ViewModels
{
    public class ProjectViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime Date { get; set; } = DateTime.Today;

        [Required]
        [Display(Name = "Customer")]
        [MaxLength(200)]
        public string Customer { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Client Type")]
        public ClientType ClientType { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required]
        [Display(Name = "Lead Channel")]
        public int LeadChannelId { get; set; }

        [Required]
        [Display(Name = "Status")]
        public ProjectStatus Status { get; set; }

        [Required]
        [Display(Name = "Amount (€)")]
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        [Display(Name = "Purchase (€)")]
        [Range(0, double.MaxValue)]
        public decimal Purchase { get; set; }

        [Required]
        [Display(Name = "Manual Margin (%)")]
        [Range(0, 100)]
        public decimal ManualMarginPercentage { get; set; }

        [Required]
        [Display(Name = "Hours")]
        [Range(0, double.MaxValue)]
        public decimal Hours { get; set; }

        [Display(Name = "Cafca Margin (%)")]
        [Range(0, 100)]
        public decimal? CafcaMarginPercentage { get; set; }

        [Display(Name = "Cafca Hours")]
        public decimal? CafcaHours { get; set; }

        [Display(Name = "Final Invoice Amount (€)")]
        public decimal? FinalInvoiceAmount { get; set; }

        [Display(Name = "End Date")]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Lost Reason")]
        [MaxLength(100)]
        public string? LostReason { get; set; }

        [Display(Name = "Notes")]
        [MaxLength(2000)]
        public string? Notes { get; set; }

        // ===== NEW: Checklist Fields =====
        [Display(Name = "In CafCa")]
        public bool CheckCafca { get; set; }

        [Display(Name = "Mapje (Folder)")]
        public bool CheckFolder { get; set; }

        [Display(Name = "Materiaal (Material)")]
        public bool CheckMaterial { get; set; }

        [Display(Name = "Planning")]
        public bool CheckPlanning { get; set; }

        // ===== NEW: Project Log =====
        public List<ProjectLogEntry> LogEntries { get; set; } = new();

        // For display
        public string? CategoryName { get; set; }
        public string? LeadChannelName { get; set; }

        // Calculated fields for display
        public decimal ManualMarginAmount => Amount * (ManualMarginPercentage / 100);
        public decimal? CafcaMarginAmount => FinalInvoiceAmount.HasValue && CafcaMarginPercentage.HasValue
            ? FinalInvoiceAmount.Value * (CafcaMarginPercentage.Value / 100)
            : null;
    }
}
