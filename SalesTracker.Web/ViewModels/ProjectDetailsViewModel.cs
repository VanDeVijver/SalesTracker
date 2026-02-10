using SalesTracker.Core.Models;

namespace SalesTracker.Web.ViewModels
{
    public class ProjectDetailsViewModel
    {
        // Basic Information
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Customer { get; set; } = string.Empty;
        public ClientType ClientType { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string LeadChannelName { get; set; } = string.Empty;
        public ProjectStatus Status { get; set; }

        // Financial - Quote
        public decimal Amount { get; set; }
        public decimal Purchase { get; set; }
        public decimal ManualMarginPercentage { get; set; }
        public decimal Hours { get; set; }

        // Financial - Actual (Cafca)
        public decimal? CafcaMarginPercentage { get; set; }
        public decimal? CafcaHours { get; set; }
        public decimal? FinalInvoiceAmount { get; set; }

        // Status Information
        public DateTime? EndDate { get; set; }
        public string? LostReason { get; set; }
        public string? Notes { get; set; }

        // Checklist
        public bool CheckCafca { get; set; }
        public bool CheckFolder { get; set; }
        public bool CheckMaterial { get; set; }
        public bool CheckPlanning { get; set; }

        // Project Log
        public List<ProjectLogEntry> LogEntries { get; set; } = new();

        // Calculated Fields
        public decimal ManualMarginAmount => Amount * (ManualMarginPercentage / 100);
        public decimal? CafcaMarginAmount => FinalInvoiceAmount.HasValue && CafcaMarginPercentage.HasValue
            ? FinalInvoiceAmount.Value * (CafcaMarginPercentage.Value / 100)
            : null;
        public decimal? MarginDifference => CafcaMarginPercentage.HasValue
            ? CafcaMarginPercentage.Value - ManualMarginPercentage
            : null;
        public decimal? HoursDifference => CafcaHours.HasValue
            ? CafcaHours.Value - Hours
            : null;
        public int? LeadTimeDays => EndDate.HasValue
            ? (EndDate.Value - Date).Days
            : null;
    }
}
