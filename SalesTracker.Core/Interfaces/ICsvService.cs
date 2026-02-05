using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesTracker.Core.Interfaces
{
    public interface ICsvService
    {
        Task<string> ExportProjectsToCsvAsync(int? year = null);
        Task<(int imported, List<string> errors)> ImportProjectsFromCsvAsync(Stream csvStream);
        Task<byte[]> GenerateCsvFileAsync(int? year = null);
    }
}
