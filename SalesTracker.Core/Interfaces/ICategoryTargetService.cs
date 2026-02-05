using SalesTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesTracker.Core.Interfaces
{
    public interface ICategoryTargetService
    {
        Task<IEnumerable<CategoryTarget>> GetTargetsByYearAsync(int year);
        Task<CategoryTarget?> GetTargetAsync(int year, int categoryId);
        Task<CategoryTarget> CreateOrUpdateTargetAsync(CategoryTarget target);
        Task DeleteTargetAsync(int id);
    }
}
