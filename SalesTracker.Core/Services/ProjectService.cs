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

            if (project.EndDate.HasValue)
            {
                existing.EndDate = DateTime.SpecifyKind(project.EndDate.Value, DateTimeKind.Utc);
            }
            else
            {
                existing.EndDate = null;
            }

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
    }
}
