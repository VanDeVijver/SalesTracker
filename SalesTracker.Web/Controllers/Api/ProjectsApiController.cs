using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SalesTracker.Core.Entities;
using SalesTracker.Core.Interfaces;
using SalesTracker.Core.Models;

namespace SalesTracker.Web.Controllers.Api
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsApiController : ControllerBase
    {
        private readonly IProjectService _projectService;
        private readonly ICategoryService _categoryService;
        private readonly ILeadChannelService _leadChannelService;
        private readonly ICategoryTargetService _targetService;

        public ProjectsApiController(
            IProjectService projectService,
            ICategoryService categoryService,
            ILeadChannelService leadChannelService,
            ICategoryTargetService targetService)
        {
            _projectService = projectService;
            _categoryService = categoryService;
            _leadChannelService = leadChannelService;
            _targetService = targetService;
        }

        // GET: api/ProjectsApi
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? year)
        {
            var projects = year.HasValue
                ? await _projectService.GetProjectsByYearAsync(year.Value)
                : await _projectService.GetAllProjectsAsync();

            var result = projects.Select(p => new
            {
                p.Id,
                p.Date,
                p.Customer,
                ClientType = p.ClientType.ToString(),
                Category = p.Category.Name,
                CategoryId = p.CategoryId,
                LeadChannel = p.LeadChannel.Name,
                LeadChannelId = p.LeadChannelId,
                Status = p.Status.ToString(),
                p.Amount,
                p.Purchase,
                p.ManualMarginPercentage, // Changed
                ManualMarginAmount = p.ManualMarginAmount, // Add calculated
                p.Hours,
                p.CafcaMarginPercentage, // Changed
                CafcaMarginAmount = p.CafcaMarginAmount, // Add calculated
                p.CafcaHours,
                p.FinalInvoiceAmount,
                p.EndDate,
                p.LostReason,
                p.Notes,
                p.CreatedAt,
                p.UpdatedAt
            });

            return Ok(result);
        }

        // GET: api/ProjectsApi/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
                return NotFound();

            var result = new
            {
                project.Id,
                project.Date,
                project.Customer,
                ClientType = project.ClientType.ToString(),
                Category = project.Category.Name,
                CategoryId = project.CategoryId,
                LeadChannel = project.LeadChannel.Name,
                LeadChannelId = project.LeadChannelId,
                Status = project.Status.ToString(),
                project.Amount,
                project.Purchase,
                project.ManualMarginPercentage, // Changed
                ManualMarginAmount = project.ManualMarginAmount, // Add calculated
                project.Hours,
                project.CafcaMarginPercentage, // Changed
                CafcaMarginAmount = project.CafcaMarginAmount, // Add calculated
                project.CafcaHours,
                project.FinalInvoiceAmount,
                project.EndDate,
                project.LostReason,
                project.Notes
            };

            return Ok(result);
        }

        // POST: api/ProjectsApi
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProjectDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var project = new Project
            {
                Date = dto.Date,
                Customer = dto.Customer,
                ClientType = Enum.Parse<ClientType>(dto.ClientType),
                CategoryId = dto.CategoryId,
                LeadChannelId = dto.LeadChannelId,
                Status = Enum.Parse<ProjectStatus>(dto.Status),
                Amount = dto.Amount,
                Purchase = dto.Purchase,
                ManualMarginPercentage = dto.ManualMarginPercentage, // Changed
                Hours = dto.Hours,
                CafcaMarginPercentage = dto.CafcaMarginPercentage, // Changed
                CafcaHours = dto.CafcaHours,
                FinalInvoiceAmount = dto.FinalInvoiceAmount,
                EndDate = dto.EndDate,
                LostReason = dto.LostReason,
                Notes = dto.Notes
            };

            var created = await _projectService.CreateProjectAsync(project);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, new
            {
                created.Id,
                created.Date,
                created.Customer,
                ClientType = created.ClientType.ToString(),
                Category = created.Category.Name,
                CategoryId = created.CategoryId,
                LeadChannel = created.LeadChannel.Name,
                LeadChannelId = created.LeadChannelId,
                Status = created.Status.ToString(),
                created.Amount,
                created.Purchase,
                created.ManualMarginPercentage, // Changed
                ManualMarginAmount = created.ManualMarginAmount, // Add calculated
                created.Hours,
                created.CafcaMarginPercentage, // Changed
                CafcaMarginAmount = created.CafcaMarginAmount, // Add calculated
                created.CafcaHours,
                created.FinalInvoiceAmount,
                created.EndDate,
                created.LostReason,
                created.Notes
            });
        }

        // PUT: api/ProjectsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ProjectDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var project = new Project
            {
                Id = id,
                Date = dto.Date,
                Customer = dto.Customer,
                ClientType = Enum.Parse<ClientType>(dto.ClientType),
                CategoryId = dto.CategoryId,
                LeadChannelId = dto.LeadChannelId,
                Status = Enum.Parse<ProjectStatus>(dto.Status),
                Amount = dto.Amount,
                Purchase = dto.Purchase,
                ManualMarginPercentage = dto.ManualMarginPercentage, // Changed
                Hours = dto.Hours,
                CafcaMarginPercentage = dto.CafcaMarginPercentage, // Changed
                CafcaHours = dto.CafcaHours,
                FinalInvoiceAmount = dto.FinalInvoiceAmount,
                EndDate = dto.EndDate,
                LostReason = dto.LostReason,
                Notes = dto.Notes
            };

            try
            {
                var updated = await _projectService.UpdateProjectAsync(project);
                return Ok(new
                {
                    updated.Id,
                    updated.Date,
                    updated.Customer,
                    ClientType = updated.ClientType.ToString(),
                    Category = updated.Category.Name,
                    CategoryId = updated.CategoryId,
                    LeadChannel = updated.LeadChannel.Name,
                    LeadChannelId = updated.LeadChannelId,
                    Status = updated.Status.ToString(),
                    updated.Amount,
                    updated.Purchase,
                    updated.ManualMarginPercentage, // Changed
                    ManualMarginAmount = updated.ManualMarginAmount, // Add calculated
                    updated.Hours,
                    updated.CafcaMarginPercentage, // Changed
                    CafcaMarginAmount = updated.CafcaMarginAmount, // Add calculated
                    updated.CafcaHours,
                    updated.FinalInvoiceAmount,
                    updated.EndDate,
                    updated.LostReason,
                    updated.Notes
                });
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/ProjectsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _projectService.DeleteProjectAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // GET: api/ProjectsApi/dashboard
        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboardStats([FromQuery] int year)
        {
            var stats = await _projectService.GetDashboardStatsAsync(year);
            var projects = await _projectService.GetProjectsByYearAsync(year);
            var targets = await _targetService.GetTargetsByYearAsync(year);

            var categoryBreakdown = projects
                .GroupBy(p => p.Category.Name)
                .Select(g => new
                {
                    Category = g.Key,
                    Won = g.Where(p => p.Status == ProjectStatus.Won).Sum(p => p.FinalInvoiceAmount ?? p.Amount),
                    Pending = g.Where(p => p.Status == ProjectStatus.Pending).Sum(p => p.Amount),
                    Lost = g.Where(p => p.Status == ProjectStatus.Lost).Sum(p => p.Amount),
                    Target = targets.FirstOrDefault(t => t.Category.Name == g.Key)?.TargetAmount ?? 0
                })
                .ToList();

            var statusBreakdown = new
            {
                Pending = projects.Count(p => p.Status == ProjectStatus.Pending),
                Won = projects.Count(p => p.Status == ProjectStatus.Won),
                Lost = projects.Count(p => p.Status == ProjectStatus.Lost)
            };

            return Ok(new
            {
                stats,
                categoryBreakdown,
                statusBreakdown
            });
        }

        // GET: api/ProjectsApi/categories
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories.Select(c => new { c.Id, c.Name }));
        }

        // GET: api/ProjectsApi/leadchannels
        [HttpGet("leadchannels")]
        public async Task<IActionResult> GetLeadChannels()
        {
            var channels = await _leadChannelService.GetAllLeadChannelsAsync();
            return Ok(channels.Select(lc => new { lc.Id, lc.Name }));
        }
    }

    // DTO for API requests
    public class ProjectDto
    {
        public DateTime Date { get; set; }
        public string Customer { get; set; } = string.Empty;
        public string ClientType { get; set; } = "New";
        public int CategoryId { get; set; }
        public int LeadChannelId { get; set; }
        public string Status { get; set; } = "Pending";
        public decimal Amount { get; set; }
        public decimal Purchase { get; set; }
        public decimal ManualMarginPercentage { get; set; } // Changed
        public decimal Hours { get; set; }
        public decimal? CafcaMarginPercentage { get; set; } // Changed
        public decimal? CafcaHours { get; set; }
        public decimal? FinalInvoiceAmount { get; set; }
        public DateTime? EndDate { get; set; }
        public string? LostReason { get; set; }
        public string? Notes { get; set; }
    }

}
