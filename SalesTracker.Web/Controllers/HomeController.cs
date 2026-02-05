using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesTracker.Core.Interfaces;
using SalesTracker.Core.Models;
using SalesTracker.Web.ViewModels;

namespace SalesTracker.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly ICategoryTargetService _targetService;

        public HomeController(
            IProjectService projectService,
            ICategoryTargetService targetService)
        {
            _projectService = projectService;
            _targetService = targetService;
        }

        public async Task<IActionResult> Index(int? year)
        {
            var currentYear = year ?? DateTime.Now.Year;

            var stats = await _projectService.GetDashboardStatsAsync(currentYear);
            var projects = await _projectService.GetProjectsByYearAsync(currentYear);
            var targets = await _targetService.GetTargetsByYearAsync(currentYear);

            var categoryBreakdown = projects
                .GroupBy(p => p.Category.Name)
                .ToDictionary(
                    g => g.Key,
                    g => g.Where(p => p.Status == ProjectStatus.Won).Sum(p => p.FinalInvoiceAmount ?? p.Amount)
                );

            var statusBreakdown = new Dictionary<string, int>
            {
                ["Pending"] = projects.Count(p => p.Status == ProjectStatus.Pending),
                ["Won"] = projects.Count(p => p.Status == ProjectStatus.Won),
                ["Lost"] = projects.Count(p => p.Status == ProjectStatus.Lost)
            };

            var recentProjects = projects
                .OrderByDescending(p => p.CreatedAt)
                .Take(10)
                .Select(p => new ProjectViewModel
                {
                    Id = p.Id,
                    Date = p.Date,
                    Customer = p.Customer,
                    CategoryName = p.Category.Name,
                    Status = p.Status,
                    Amount = p.Amount,
                    FinalInvoiceAmount = p.FinalInvoiceAmount
                })
                .ToList();

            var viewModel = new DashboardViewModel
            {
                CurrentYear = currentYear,
                Stats = stats,
                CategoryBreakdown = categoryBreakdown,
                StatusBreakdown = statusBreakdown,
                RecentProjects = recentProjects
            };

            ViewBag.Years = Enumerable.Range(2020, DateTime.Now.Year - 2019).Reverse();

            return View(viewModel);
        }
    }
}
