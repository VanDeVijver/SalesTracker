using SalesTracker.Web.Models;

namespace SalesTracker.Web.ViewModels
{
    public class DashboardViewModel
    {
        public int CurrentYear { get; set; }
        public Dictionary<string, decimal> Stats { get; set; } = new();
        public List<ProjectViewModel> RecentProjects { get; set; } = new();
        public Dictionary<string, decimal> CategoryBreakdown { get; set; } = new();
        public Dictionary<string, int> StatusBreakdown { get; set; } = new();

        public int? Month { get; set; }
        public string? CategoryFilter { get; set; } = "all";

        public decimal TotalWonRevenue { get; set; }
        public decimal TotalPipelineValue { get; set; }
        public decimal AverageMargin { get; set; }
        public int AverageLeadTime { get; set; }
        public decimal TargetPercentage { get; set; }
        public decimal YearlyTarget { get; set; }

        public string PipelineChartData { get; set; } = "{}";
        public string CategoryChartData { get; set; } = "{}";
        public string ConversionChartData { get; set; } = "{}";
        public string MarginChartData { get; set; } = "{}";
        public string LeadTimeChartData { get; set; } = "{}";
        public string LostReasonsChartData { get; set; } = "{}";

        public List<string> Categories { get; set; } = new();
        public List<FinancialAnalysisItem> Leakage { get; set; } = new();
        public List<FinancialAnalysisItem> Success { get; set; } = new();

    }
}
