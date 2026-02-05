using Microsoft.EntityFrameworkCore;
using SalesTracker.Core.Data;
using SalesTracker.Core.Entities;
using SalesTracker.Core.Interfaces;

namespace SalesTracker.Core.Services
{
    public class LeadChannelService : ILeadChannelService
    {
        private readonly ApplicationDbContext _context;

        public LeadChannelService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeadChannel>> GetAllLeadChannelsAsync()
        {
            return await _context.LeadChannels
                .Where(lc => lc.IsActive)
                .OrderBy(lc => lc.Name)
                .ToListAsync();
        }

        public async Task<LeadChannel?> GetLeadChannelByIdAsync(int id)
        {
            return await _context.LeadChannels.FindAsync(id);
        }

        public async Task<LeadChannel> CreateLeadChannelAsync(LeadChannel leadChannel)
        {
            leadChannel.CreatedAt = DateTime.UtcNow;
            _context.LeadChannels.Add(leadChannel);
            await _context.SaveChangesAsync();
            return leadChannel;
        }

        public async Task<LeadChannel> UpdateLeadChannelAsync(LeadChannel leadChannel)
        {
            _context.LeadChannels.Update(leadChannel);
            await _context.SaveChangesAsync();
            return leadChannel;
        }

        public async Task DeleteLeadChannelAsync(int id)
        {
            var leadChannel = await _context.LeadChannels.FindAsync(id);
            if (leadChannel != null)
            {
                leadChannel.IsActive = false;
                await _context.SaveChangesAsync();
            }
        }
    }
}
