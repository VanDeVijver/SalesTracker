using SalesTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesTracker.Core.Interfaces
{
    public interface ILeadChannelService
    {
        Task<IEnumerable<LeadChannel>> GetAllLeadChannelsAsync();
        Task<LeadChannel?> GetLeadChannelByIdAsync(int id);
        Task<LeadChannel> CreateLeadChannelAsync(LeadChannel leadChannel);
        Task<LeadChannel> UpdateLeadChannelAsync(LeadChannel leadChannel);
        Task DeleteLeadChannelAsync(int id);
    }
}
