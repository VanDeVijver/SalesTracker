using Microsoft.EntityFrameworkCore;
using SalesTracker.Core.Data;
using SalesTracker.Core.Entities;
using SalesTracker.Core.Interfaces;

namespace SalesTracker.Core.Services
{
    public class CategoryTargetService : ICategoryTargetService
    {
        private readonly ApplicationDbContext _context;

        public CategoryTargetService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryTarget>> GetTargetsByYearAsync(int year)
        {
            return await _context.CategoryTargets
                .Include(ct => ct.Category)
                .Where(ct => ct.Year == year)
                .ToListAsync();
        }

        public async Task<CategoryTarget?> GetTargetAsync(int year, int categoryId)
        {
            return await _context.CategoryTargets
                .Include(ct => ct.Category)
                .FirstOrDefaultAsync(ct => ct.Year == year && ct.CategoryId == categoryId);
        }

        public async Task<CategoryTarget> CreateOrUpdateTargetAsync(CategoryTarget target)
        {
            var existing = await GetTargetAsync(target.Year, target.CategoryId);

            if (existing != null)
            {
                existing.TargetAmount = target.TargetAmount;
                existing.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                return existing;
            }
            else
            {
                target.CreatedAt = DateTime.UtcNow;
                _context.CategoryTargets.Add(target);
                await _context.SaveChangesAsync();
                return target;
            }
        }

        public async Task DeleteTargetAsync(int id)
        {
            var target = await _context.CategoryTargets.FindAsync(id);
            if (target != null)
            {
                _context.CategoryTargets.Remove(target);
                await _context.SaveChangesAsync();
            }
        }
    }
}
