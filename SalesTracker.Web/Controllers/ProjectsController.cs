using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SalesTracker.Core.Entities;
using SalesTracker.Core.Interfaces;
using SalesTracker.Web.ViewModels;

namespace SalesTracker.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly ICategoryService _categoryService;
        private readonly ILeadChannelService _leadChannelService;
        private readonly ICsvService _csvService;

        public ProjectsController(
            IProjectService projectService,
            ICategoryService categoryService,
            ILeadChannelService leadChannelService,
            ICsvService csvService)
        {
            _projectService = projectService;
            _categoryService = categoryService;
            _leadChannelService = leadChannelService;
            _csvService = csvService;
        }

        // GET: Projects
        public async Task<IActionResult> Index(int? year)
        {
            var currentYear = year ?? DateTime.Now.Year;
            var projects = await _projectService.GetProjectsByYearAsync(currentYear);

            var viewModels = projects.Select(p => new ProjectViewModel
            {
                Id = p.Id,
                Date = p.Date,
                Customer = p.Customer,
                ClientType = p.ClientType,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                LeadChannelId = p.LeadChannelId,
                LeadChannelName = p.LeadChannel.Name,
                Status = p.Status,
                Amount = p.Amount,
                Purchase = p.Purchase,
                ManualMarginPercentage = p.ManualMarginPercentage, // Changed
                Hours = p.Hours,
                CafcaMarginPercentage = p.CafcaMarginPercentage, // Changed
                CafcaHours = p.CafcaHours,
                FinalInvoiceAmount = p.FinalInvoiceAmount,
                EndDate = p.EndDate,
                LostReason = p.LostReason,
                Notes = p.Notes
            }).ToList();

            ViewBag.CurrentYear = currentYear;
            ViewBag.Years = Enumerable.Range(2020, DateTime.Now.Year - 2019).Reverse();

            return View(viewModels);
        }

        // GET: Projects/Create
        public async Task<IActionResult> Create()
        {
            await PopulateDropdowns();
            return View(new ProjectViewModel { Date = DateTime.Today });
        }

        // POST: Projects/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProjectViewModel model)
        {
            if (ModelState.IsValid)
            {
                var project = new Project
                {
                    Date = model.Date,
                    Customer = model.Customer,
                    ClientType = model.ClientType,
                    CategoryId = model.CategoryId,
                    LeadChannelId = model.LeadChannelId,
                    Status = model.Status,
                    Amount = model.Amount,
                    Purchase = model.Purchase,
                    ManualMarginPercentage = model.ManualMarginPercentage,
                    Hours = model.Hours,
                    CafcaMarginPercentage = model.CafcaMarginPercentage,
                    CafcaHours = model.CafcaHours,
                    FinalInvoiceAmount = model.FinalInvoiceAmount,
                    EndDate = model.EndDate,
                    LostReason = model.LostReason,
                    Notes = model.Notes
                };

                await _projectService.CreateProjectAsync(project);
                TempData["Success"] = "Project created successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns();
            return View(model);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
                return NotFound();

            var model = new ProjectViewModel
            {
                Id = project.Id,
                Date = project.Date,
                Customer = project.Customer,
                ClientType = project.ClientType,
                CategoryId = project.CategoryId,
                LeadChannelId = project.LeadChannelId,
                Status = project.Status,
                Amount = project.Amount,
                Purchase = project.Purchase,
                ManualMarginPercentage = project.ManualMarginPercentage,
                Hours = project.Hours,
                CafcaMarginPercentage = project.CafcaMarginPercentage,
                CafcaHours = project.CafcaHours,
                FinalInvoiceAmount = project.FinalInvoiceAmount,
                EndDate = project.EndDate,
                LostReason = project.LostReason,
                Notes = project.Notes
            };

            await PopulateDropdowns();
            return View(model);
        }

        // POST: Projects/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProjectViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var project = new Project
                {
                    Id = model.Id,
                    Date = model.Date,
                    Customer = model.Customer,
                    ClientType = model.ClientType,
                    CategoryId = model.CategoryId,
                    LeadChannelId = model.LeadChannelId,
                    Status = model.Status,
                    Amount = model.Amount,
                    Purchase = model.Purchase,
                    ManualMarginPercentage = model.ManualMarginPercentage, // Changed
                    Hours = model.Hours,
                    CafcaMarginPercentage = model.CafcaMarginPercentage, // Changed
                    CafcaHours = model.CafcaHours,
                    FinalInvoiceAmount = model.FinalInvoiceAmount,
                    EndDate = model.EndDate,
                    LostReason = model.LostReason,
                    Notes = model.Notes
                };

                await _projectService.UpdateProjectAsync(project);
                TempData["Success"] = "Project updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            await PopulateDropdowns();
            return View(model);
        }

        // POST: Projects/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _projectService.DeleteProjectAsync(id);
            TempData["Success"] = "Project deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        // GET: Projects/ExportCsv
        public async Task<IActionResult> ExportCsv(int? year)
        {
            var csvBytes = await _csvService.GenerateCsvFileAsync(year);
            var fileName = year.HasValue
                ? $"projects_{year.Value}.csv"
                : $"projects_all.csv";

            return File(csvBytes, "text/csv", fileName);
        }

        // POST: Projects/ImportCsv
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportCsv(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                TempData["Error"] = "Please select a CSV file to import.";
                return RedirectToAction(nameof(Index));
            }

            using var stream = file.OpenReadStream();
            var (imported, errors) = await _csvService.ImportProjectsFromCsvAsync(stream);

            if (errors.Any())
            {
                TempData["Warning"] = $"Imported {imported} projects with {errors.Count} errors.";
                TempData["Errors"] = string.Join("<br/>", errors);
            }
            else
            {
                TempData["Success"] = $"Successfully imported {imported} projects!";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateDropdowns()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            var leadChannels = await _leadChannelService.GetAllLeadChannelsAsync();

            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewBag.LeadChannels = new SelectList(leadChannels, "Id", "Name");
        }
    }
}
