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
        Task<Dictionary<string, decimal>> GetDashboardStatsAsync(int year);
    }
}
