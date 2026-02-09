using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesTracker.Core.Interfaces;
using SalesTracker.Core.Models;
using SalesTracker.Web.Models;
using SalesTracker.Web.ViewModels;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SalesTracker.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly ICategoryService _categoryService;
        private readonly ICategoryTargetService _categoryTargetService;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public HomeController(
            IProjectService projectService,
            ICategoryService categoryService,
            ICategoryTargetService categoryTargetService)
        {
            _projectService = projectService;
            _categoryService = categoryService;
            _categoryTargetService = categoryTargetService;
        }

        public async Task<IActionResult> Index(int? year, int? month, string? category)
        {
            var currentYear = year ?? DateTime.Now.Year;
            var categoryFilter = category ?? "all";

            // Get all projects for the year
            var allProjects = await _projectService.GetProjectsByYearAsync(currentYear);

            // Apply filters
            var filteredProjects = allProjects.Where(p =>
            {
                var projectDate = p.Status == ProjectStatus.Won ? p.EndDate : p.Date;
                if (!projectDate.HasValue) projectDate = p.Date;

                var matchesMonth = !month.HasValue || projectDate.Value.Month == month.Value;
                var matchesCategory = categoryFilter == "all" || p.Category.Name == categoryFilter;

                return matchesMonth && matchesCategory;
            }).ToList();

            var wonProjects = filteredProjects.Where(p => p.Status == ProjectStatus.Won).ToList();
            var pendingProjects = filteredProjects.Where(p => p.Status == ProjectStatus.Pending).ToList();
            var lostProjects = filteredProjects.Where(p => p.Status == ProjectStatus.Lost).ToList();

            // Get categories
            var categories = await _categoryService.GetAllCategoriesAsync();
            var categoryNames = categories.Select(c => c.Name).ToList();

            var activeCategories = categoryFilter == "all"
                ? categoryNames
                : new List<string> { categoryFilter };

            // Get targets
            var yearlyTargets = (await _categoryTargetService.GetTargetsByYearAsync(currentYear)).ToList();
            var yearlyTargetTotal = yearlyTargets.Sum(t => t.TargetAmount);
            var targetDivisor = month.HasValue ? 12m : 1m;
            var activeTarget = categoryFilter == "all"
                ? yearlyTargetTotal / targetDivisor
                : (yearlyTargets.FirstOrDefault(t => t.Category.Name == categoryFilter)?.TargetAmount ?? 0) / targetDivisor;

            // Calculate stats (existing functionality)
            var stats = await _projectService.GetDashboardStatsAsync(currentYear);

            // Build ViewModel
            var model = new DashboardViewModel
            {
                CurrentYear = currentYear,
                Month = month,
                CategoryFilter = categoryFilter,
                Stats = stats,
                Categories = categoryNames,

                // KPIs
                TotalWonRevenue = wonProjects.Sum(p => p.FinalInvoiceAmount ?? p.Amount),
                TotalPipelineValue = pendingProjects.Sum(p => p.Amount),
                YearlyTarget = yearlyTargetTotal,

                // Average Margin
                AverageMargin = wonProjects.Where(p => p.CafcaMarginPercentage.HasValue).Any()
                    ? wonProjects.Where(p => p.CafcaMarginPercentage.HasValue).Average(p => p.CafcaMarginPercentage!.Value)
                    : 0,

                // Average Lead Time
                AverageLeadTime = CalculateAverageLeadTime(wonProjects),

                // Target Percentage
                TargetPercentage = activeTarget > 0
                    ? Math.Round((wonProjects.Sum(p => p.FinalInvoiceAmount ?? p.Amount) / activeTarget) * 100, 2)
                    : 0,

                // Recent Projects (existing)
                RecentProjects = allProjects
                    .OrderByDescending(p => p.Date)
                    .Take(10)
                    .Select(p => new ProjectViewModel
                    {
                        Id = p.Id,
                        Date = p.Date,
                        Customer = p.Customer,
                        CategoryName = p.Category.Name,
                        LeadChannelName = p.LeadChannel.Name,
                        Status = p.Status,
                        Amount = p.Amount
                    }).ToList(),

                // Category Breakdown (existing)
                CategoryBreakdown = allProjects
                    .GroupBy(p => p.Category.Name)
                    .ToDictionary(g => g.Key, g => g.Sum(p => p.Amount)),

                // Status Breakdown (existing)
                StatusBreakdown = allProjects
                    .GroupBy(p => p.Status)
                    .ToDictionary(g => g.Key.ToString(), g => g.Count()),

                // Chart Data
                PipelineChartData = GeneratePipelineChartData(pendingProjects, activeCategories),
                CategoryChartData = GenerateCategoryChartData(wonProjects, activeCategories, yearlyTargets, targetDivisor),
                ConversionChartData = GenerateConversionChartData(filteredProjects, wonProjects, activeCategories),
                MarginChartData = GenerateMarginChartData(wonProjects, activeCategories),
                LeadTimeChartData = GenerateLeadTimeChartData(wonProjects, activeCategories),
                LostReasonsChartData = GenerateLostReasonsChartData(lostProjects),

                // Financial Analysis
                Leakage = GenerateFinancialAnalysis(wonProjects, 50, true),
                Success = GenerateFinancialAnalysis(wonProjects, 50, false)
            };

            return View(model);
        }

        private int CalculateAverageLeadTime(List<Core.Entities.Project> wonProjects)
        {
            var leadTimes = wonProjects
                .Where(p => p.EndDate.HasValue)
                .Select(p => (p.EndDate!.Value - p.Date).Days)
                .Where(d => d >= 0)
                .ToList();

            return leadTimes.Any() ? (int)leadTimes.Average() : 0;
        }

        private string GeneratePipelineChartData(List<Core.Entities.Project> pendingProjects, List<string> categories)
        {
            var data = new ChartData
            {
                Labels = categories,
                Datasets = new List<ChartDataset>
                {
                    new ChartDataset
                    {
                        Label = "Pipeline €",
                        Data = categories.Select(c =>
                            pendingProjects.Where(p => p.Category.Name == c).Sum(p => p.Amount)
                        ).ToList(),
                        BackgroundColor = "#3b82f6"
                    }
                }
            };

            return JsonSerializer.Serialize(data, JsonOptions);
        }

        private string GenerateCategoryChartData(
            List<Core.Entities.Project> wonProjects,
            List<string> categories,
            List<Core.Entities.CategoryTarget> targets,
            decimal targetDivisor)
        {
            var data = new ChartData
            {
                Labels = categories,
                Datasets = new List<ChartDataset>
                {
                    new ChartDataset
                    {
                        Label = "Behaald",
                        Data = categories.Select(c =>
                            wonProjects.Where(p => p.Category.Name == c)
                                .Sum(p => p.FinalInvoiceAmount ?? p.Amount)
                        ).ToList(),
                        BackgroundColor = "#10b981"
                    },
                    new ChartDataset
                    {
                        Label = "Target",
                        Data = categories.Select(c =>
                            (targets.FirstOrDefault(t => t.Category.Name == c)?.TargetAmount ?? 0) / targetDivisor
                        ).ToList(),
                        BackgroundColor = "#e2e8f0"
                    }
                }
            };

            return JsonSerializer.Serialize(data, JsonOptions);
        }

        private string GenerateConversionChartData(
            List<Core.Entities.Project> allProjects,
            List<Core.Entities.Project> wonProjects,
            List<string> categories)
        {
            var data = new ChartData
            {
                Labels = categories,
                Datasets = new List<ChartDataset>
                {
                    new ChartDataset
                    {
                        Label = "Conversie %",
                        Data = categories.Select(c =>
                        {
                            var total = allProjects.Count(p => p.Category.Name == c);
                            var won = wonProjects.Count(p => p.Category.Name == c);
                            return total > 0 ? Math.Round((decimal)won / total * 100, 2) : 0;
                        }).ToList(),
                        BackgroundColor = "#6366f1"
                    }
                }
            };

            return JsonSerializer.Serialize(data, JsonOptions);
        }

        private string GenerateMarginChartData(List<Core.Entities.Project> wonProjects, List<string> categories)
        {
            var data = new ChartData
            {
                Labels = categories,
                Datasets = new List<ChartDataset>
                {
                    new ChartDataset
                    {
                        Label = "Marge %",
                        Data = categories.Select(c =>
                        {
                            var projectsWithMargin = wonProjects
                                .Where(p => p.Category.Name == c && p.CafcaMarginPercentage.HasValue)
                                .ToList();
                            return projectsWithMargin.Any()
                                ? Math.Round(projectsWithMargin.Average(p => p.CafcaMarginPercentage!.Value), 2)
                                : 0;
                        }).ToList(),
                        BackgroundColor = "#f97316"
                    }
                }
            };

            return JsonSerializer.Serialize(data, JsonOptions);
        }

        private string GenerateLeadTimeChartData(List<Core.Entities.Project> wonProjects, List<string> categories)
        {
            var data = new ChartData
            {
                Labels = categories,
                Datasets = new List<ChartDataset>
        {
            new ChartDataset
            {
                Label = "Dagen",
                Data = categories.Select(c =>
                {
                    var leadTimes = wonProjects
                        .Where(p => p.Category.Name == c && p.EndDate.HasValue)
                        .Select(p => (decimal)(p.EndDate!.Value - p.Date).Days)
                        .Where(d => d >= 0)
                        .ToList();
                    return leadTimes.Any() ? Math.Round(leadTimes.Average(), 0) : 0;
                }).ToList(),
                BackgroundColor = "#8b5cf6",
                BorderColor = "#7c3aed" // Add border color for better visibility
            }
        }
            };

            return JsonSerializer.Serialize(data, JsonOptions);
        }
        private string GenerateLostReasonsChartData(List<Core.Entities.Project> lostProjects)
        {
            var reasons = new[] { "Prijs", "Concurrent", "Geannuleerd", "Uitgesteld", "Te laat", "Other" };

            var data = new ChartData
            {
                Labels = reasons.ToList(),
                Datasets = new List<ChartDataset>
                {
                    new ChartDataset
                    {
                        Label = "Reden Lost",
                        Data = reasons.Select(r =>
                        {
                            if (r == "Other")
                            {
                                return (decimal)lostProjects.Count(p =>
                                    string.IsNullOrEmpty(p.LostReason) ||
                                    !reasons.Take(5).Contains(p.LostReason));
                            }
                            return (decimal)lostProjects.Count(p => p.LostReason == r);
                        }).ToList(),
                        BackgroundColor = "rgba(239, 68, 68, 0.2)",
                        BorderColor = "#ef4444"
                    }
                }
            };

            return JsonSerializer.Serialize(data, JsonOptions);
        }

        private List<FinancialAnalysisItem> GenerateFinancialAnalysis(
            List<Core.Entities.Project> wonProjects,
            decimal hourlyRate,
            bool isLeakage)
        {
            return wonProjects
                .Where(p => p.CafcaMarginPercentage.HasValue)
                .Select(p =>
                {
                    var baseAmount = p.Amount;
                    var actualAmount = p.FinalInvoiceAmount ?? baseAmount;
                    var baseMarginEuro = baseAmount * (p.ManualMarginPercentage / 100);
                    var actualMarginEuro = actualAmount * (p.CafcaMarginPercentage!.Value / 100);
                    var laborImpact = ((p.CafcaHours ?? 0) - p.Hours) * hourlyRate;
                    var totalImpact = (actualMarginEuro - baseMarginEuro) - laborImpact;

                    return new FinancialAnalysisItem
                    {
                        Customer = p.Customer,
                        MarginDifference = p.CafcaMarginPercentage.Value - p.ManualMarginPercentage,
                        ManualMargin = p.ManualMarginPercentage,
                        CafcaMargin = p.CafcaMarginPercentage.Value,
                        HoursDifference = (p.CafcaHours ?? 0) - p.Hours,
                        ManualHours = p.Hours,
                        CafcaHours = p.CafcaHours ?? 0,
                        LaborImpact = laborImpact,
                        TotalImpact = totalImpact
                    };
                })
                .Where(item => isLeakage ? item.TotalImpact < -1 : item.TotalImpact > 1)
                .OrderBy(item => item.TotalImpact)
                .ToList();
        }
    }
}
