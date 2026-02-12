using Microsoft.AspNetCore.Mvc.Rendering;
using SalesTracker.Core.Entities;
using SalesTracker.Web.Models;

namespace SalesTracker.Web.ViewModels
{
    public class DashboardViewModel
    {
        public int CurrentYear { get; set; }
        public int? Month { get; set; }
        public int? CategoryId { get; set; }  // ✅ Add this
        public string CategoryFilter { get; set; } = "all";

        public Dictionary<string, decimal> Stats { get; set; } = new();
        public List<string> Categories { get; set; } = new();
        public SelectList CategoriesSelectList { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());  // ✅ Add this

        // KPIs
        public decimal TotalWonRevenue { get; set; }
        public decimal TotalPipelineValue { get; set; }
        public decimal YearlyTarget { get; set; }
        public decimal AverageMargin { get; set; }
        public decimal AverageLeadTime { get; set; }
        public decimal TargetPercentage { get; set; }

        // Lists
        public List<ProjectViewModel> RecentProjects { get; set; } = new();
        public Dictionary<string, decimal> CategoryBreakdown { get; set; } = new();
        public Dictionary<string, int> StatusBreakdown { get; set; } = new();

        // Chart Data
        public string PipelineChartData { get; set; } = string.Empty;
        public string CategoryChartData { get; set; } = string.Empty;
        public string ConversionChartData { get; set; } = string.Empty;
        public string MarginChartData { get; set; } = string.Empty;
        public string LeadTimeChartData { get; set; } = string.Empty;
        public string LostReasonsChartData { get; set; } = string.Empty;
        public List<FinancialAnalysisItem> Leakage { get; set; } = new();
        public List<FinancialAnalysisItem> Success { get; set; } = new();

        // Helper property for filter description
        public string FilterDescription
        {
            get
            {
                var parts = new List<string> { CurrentYear.ToString() };

                if (Month.HasValue)
                {
                    var monthName = new DateTime(CurrentYear, Month.Value, 1).ToString("MMMM");
                    parts.Add(monthName);
                }

                if (CategoryId.HasValue && CategoryFilter != "all")
                {
                    parts.Add(CategoryFilter);
                }

                return string.Join(" - ", parts);
            }

        }
    }
}
