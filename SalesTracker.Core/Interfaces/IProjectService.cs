using SalesTracker.Core.Entities;
using SalesTracker.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesTracker.Core.Interfaces
{
    public interface IProjectService
    {
        Task<IEnumerable<Project>> GetAllProjectsAsync();
        Task<Project?> GetProjectByIdAsync(int id);
        Task<Project> CreateProjectAsync(Project project);
        Task<Project> UpdateProjectAsync(Project project);
        Task DeleteProjectAsync(int id);
        Task<IEnumerable<Project>> GetProjectsByYearAsync(int year);
        Task<IEnumerable<Project>> GetProjectsByCategoryAsync(int categoryId);
        Task<IEnumerable<Project>> GetProjectsByStatusAsync(ProjectStatus status);
        Task<IEnumerable<ProjectStatus>> GetAllStatusesByProjects(List<Project> projects);
        Task<Dictionary<string, decimal>> GetDashboardStatsAsync(int year);


        // New method for filtered and paginated results
        Task<(List<Project> projects, int totalCount)> GetFilteredProjectsAsync(
            int? year = null,
            int? categoryId = null,
            int? leadChannelId = null,
            string? status = null,
            string? searchTerm = null,
            int page = 1,
            int pageSize = 10);
    }
}
