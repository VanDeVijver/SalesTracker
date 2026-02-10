using Microsoft.EntityFrameworkCore;
using SalesTracker.Core.Data;
using SalesTracker.Core.Entities;
using SalesTracker.Core.Interfaces;
using SalesTracker.Core.Models;

namespace SalesTracker.Core.Services
{
    public class ProjectService : IProjectService
    {
        private readonly ApplicationDbContext _context;

        public ProjectService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            return await _context.Projects
                .Include(p => p.Category)
                .Include(p => p.LeadChannel)
                .OrderByDescending(p => p.Date)
                .ToListAsync();
        }

        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            return await _context.Projects
                .Include(p => p.Category)
                .Include(p => p.LeadChannel)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            // Ensure DateTime is UTC
            project.Date = DateTime.SpecifyKind(project.Date, DateTimeKind.Utc);
            project.CreatedAt = DateTime.UtcNow;

            if (project.EndDate.HasValue)
            {
                project.EndDate = DateTime.SpecifyKind(project.EndDate.Value, DateTimeKind.Utc);
            }
            // Ensure LogEntries is initialized
            if (project.LogEntries == null)
            {
                project.LogEntries = new List<Core.Models.ProjectLogEntry>();
            }

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            // Reload with navigation properties
            return (await GetProjectByIdAsync(project.Id))!;
        }

        public async Task<Project> UpdateProjectAsync(Project project)
        {
            var existing = await _context.Projects.FindAsync(project.Id);
            if (existing == null)
                throw new KeyNotFoundException($"Project with ID {project.Id} not found");

            // Update properties
            existing.Date = DateTime.SpecifyKind(project.Date, DateTimeKind.Utc);
            existing.Customer = project.Customer;
            existing.ClientType = project.ClientType;
            existing.CategoryId = project.CategoryId;
            existing.LeadChannelId = project.LeadChannelId;
            existing.Status = project.Status;
            existing.Amount = project.Amount;
            existing.Purchase = project.Purchase;
            existing.ManualMarginPercentage = project.ManualMarginPercentage;
            existing.Hours = project.Hours;
            existing.CafcaMarginPercentage = project.CafcaMarginPercentage;
            existing.CafcaHours = project.CafcaHours;
            existing.FinalInvoiceAmount = project.FinalInvoiceAmount;
            existing.Notes = project.Notes;
            existing.LostReason = project.LostReason;
            existing.UpdatedAt = DateTime.UtcNow;
            existing.EndDate = project.EndDate;
            existing.CheckCafca = project.CheckCafca;
            existing.CheckFolder = project.CheckFolder;
            existing.CheckMaterial = project.CheckMaterial;
            existing.CheckPlanning = project.CheckPlanning;
            existing.ProjectLog = project.ProjectLog;

            if (project.EndDate.HasValue)
            {
                existing.EndDate = DateTime.SpecifyKind(project.EndDate.Value, DateTimeKind.Utc);
            }
            else
            {
                existing.EndDate = null;
            }

            _context.Entry(existing).State = EntityState.Modified;

            // Explicitly mark LogEntries property as modified
            _context.Entry(existing).Property(p => p.LogEntries).IsModified = true;

            await _context.SaveChangesAsync();

            // Reload with navigation properties
            return (await GetProjectByIdAsync(existing.Id))!;
        }

        public async Task DeleteProjectAsync(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Project>> GetProjectsByYearAsync(int year)
        {
            return await _context.Projects
                .Include(p => p.Category)
                .Include(p => p.LeadChannel)
                .Where(p => p.Date.Year == year)
                .OrderByDescending(p => p.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetProjectsByCategoryAsync(int categoryId)
        {
            return await _context.Projects
                .Include(p => p.Category)
                .Include(p => p.LeadChannel)
                .Where(p => p.CategoryId == categoryId)
                .OrderByDescending(p => p.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Project>> GetProjectsByStatusAsync(ProjectStatus status)
        {
            return await _context.Projects
                .Include(p => p.Category)
                .Include(p => p.LeadChannel)
                .Where(p => p.Status == status)
                .OrderByDescending(p => p.Date)
                .ToListAsync();
        }

        public async Task<Dictionary<string, decimal>> GetDashboardStatsAsync(int year)
        {
            var projects = await GetProjectsByYearAsync(year);

            var stats = new Dictionary<string, decimal>
            {
                ["TotalProjects"] = projects.Count(),
                ["PendingProjects"] = projects.Count(p => p.Status == ProjectStatus.Pending),
                ["WonProjects"] = projects.Count(p => p.Status == ProjectStatus.Won),
                ["LostProjects"] = projects.Count(p => p.Status == ProjectStatus.Lost),
                ["TotalRevenue"] = projects.Where(p => p.Status == ProjectStatus.Won).Sum(p => p.FinalInvoiceAmount ?? p.Amount),
                ["TotalPurchase"] = projects.Where(p => p.Status == ProjectStatus.Won).Sum(p => p.Purchase),
                ["TotalMargin"] = projects.Where(p => p.Status == ProjectStatus.Won).Sum(p => p.CafcaMarginAmount ?? p.ManualMarginAmount),
                ["PendingValue"] = projects.Where(p => p.Status == ProjectStatus.Pending).Sum(p => p.Amount),
                ["WonValue"] = projects.Where(p => p.Status == ProjectStatus.Won).Sum(p => p.FinalInvoiceAmount ?? p.Amount),
                ["LostValue"] = projects.Where(p => p.Status == ProjectStatus.Lost).Sum(p => p.Amount)
            };

            return stats;
        }

        public async Task<(List<Project> projects, int totalCount)> GetFilteredProjectsAsync(
            int? year = null,
            int? categoryId = null,
            int? leadChannelId = null,
            string? status = null,
            string? searchTerm = null,
            int page = 1,
            int pageSize = 10)
        {
            var query = _context.Projects
                .Include(p => p.Category)
                .Include(p => p.LeadChannel)
                .AsQueryable();

            // Apply filters
            if (year.HasValue)
            {
                query = query.Where(p => p.Date.Year == year.Value);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }

            if (leadChannelId.HasValue)
            {
                query = query.Where(p => p.LeadChannelId == leadChannelId.Value);
            }

            if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<ProjectStatus>(status, out var projectStatus))
            {
                query = query.Where(p => p.Status == projectStatus);
            }

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(p => p.Customer.Contains(searchTerm) ||
                                        (p.Notes != null && p.Notes.Contains(searchTerm)));
            }

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Apply sorting and pagination
            var projects = await query
                .OrderByDescending(p => p.Date)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (projects, totalCount);
        }

        /// <summary>
        /// Gets all distinct statuses from the provided list of projects
        /// </summary>
        /// <param name="projects">List of projects to extract statuses from</param>
        /// <returns>Collection of distinct ProjectStatus values found in the projects</returns>
        public async Task<IEnumerable<ProjectStatus>> GetAllStatusesByProjects(List<Project> projects)
        {
            if (projects == null || !projects.Any())
            {
                return Enumerable.Empty<ProjectStatus>();
            }

            // If projects are already loaded in memory
            var statuses = projects
                .Select(p => p.Status)
                .Distinct()
                .OrderBy(s => s)
                .ToList();

            return await Task.FromResult(statuses);
        }
    }
}
