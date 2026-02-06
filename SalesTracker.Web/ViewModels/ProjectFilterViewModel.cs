using Microsoft.AspNetCore.Mvc.Rendering;

namespace SalesTracker.Web.ViewModels
{
    public class ProjectFilterViewModel
    {
        public int? Year { get; set; }
        public int? CategoryId { get; set; }
        public int? LeadChannelId { get; set; }
        public string? Status { get; set; }
        public string? SearchTerm { get; set; }

        // Pagination
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        // Data
        public List<ProjectViewModel> Projects { get; set; } = new();

        // Dropdowns
        public SelectList? Categories { get; set; }
        public SelectList? LeadChannels { get; set; }
        public SelectList? Years { get; set; }
        public SelectList? Statuses { get; set; }
    }
}
