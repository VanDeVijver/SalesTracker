namespace SalesTracker.Web.Models
{
    public class FinancialAnalysisItem
    {
        public string Customer { get; set; } = string.Empty;
        public decimal MarginDifference { get; set; }
        public decimal ManualMargin { get; set; }
        public decimal CafcaMargin { get; set; }
        public decimal HoursDifference { get; set; }
        public decimal ManualHours { get; set; }
        public decimal CafcaHours { get; set; }
        public decimal LaborImpact { get; set; }
        public decimal TotalImpact { get; set; }
    }
}
