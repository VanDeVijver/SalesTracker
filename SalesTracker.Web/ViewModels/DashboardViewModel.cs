namespace SalesTracker.Web.ViewModels
{
    public class DashboardViewModel
    {
        public int CurrentYear { get; set; }
        public Dictionary<string, decimal> Stats { get; set; } = new();
        public List<ProjectViewModel> RecentProjects { get; set; } = new();
        public Dictionary<string, decimal> CategoryBreakdown { get; set; } = new();
        public Dictionary<string, int> StatusBreakdown { get; set; } = new();
    }
}
