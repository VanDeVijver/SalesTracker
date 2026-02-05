using System.Globalization;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SalesTracker.Core.Data;
using SalesTracker.Core.Entities;
using SalesTracker.Core.Interfaces;
using SalesTracker.Core.Models;

namespace SalesTracker.Core.Services
{
    public class CsvService : ICsvService
    {
        private readonly ApplicationDbContext _context;

        public CsvService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> ExportProjectsToCsvAsync(int? year = null)
        {
            var query = _context.Projects
                .Include(p => p.Category)
                .Include(p => p.LeadChannel)
                .AsQueryable();

            if (year.HasValue)
            {
                query = query.Where(p => p.Date.Year == year.Value);
            }

            var projects = await query.OrderByDescending(p => p.Date).ToListAsync();

            var csv = new StringBuilder();

            // Header - Updated
            csv.AppendLine("Date,Customer,ClientType,Category,LeadChannel,Status,Amount,Purchase,ManualMarginPercentage,Hours,CafcaMarginPercentage,CafcaHours,FinalInvoiceAmount,EndDate,LostReason,Notes");

            // Data rows
            foreach (var project in projects)
            {
                csv.AppendLine(string.Join(",",
                    EscapeCsvField(project.Date.ToString("yyyy-MM-dd")),
                    EscapeCsvField(project.Customer),
                    EscapeCsvField(project.ClientType.ToString()),
                    EscapeCsvField(project.Category.Name),
                    EscapeCsvField(project.LeadChannel.Name),
                    EscapeCsvField(project.Status.ToString()),
                    project.Amount.ToString(CultureInfo.InvariantCulture),
                    project.Purchase.ToString(CultureInfo.InvariantCulture),
                    project.ManualMarginPercentage.ToString(CultureInfo.InvariantCulture), // Changed
                    project.Hours.ToString(CultureInfo.InvariantCulture),
                    (project.CafcaMarginPercentage?.ToString(CultureInfo.InvariantCulture) ?? ""), // Changed
                    (project.CafcaHours?.ToString(CultureInfo.InvariantCulture) ?? ""),
                    (project.FinalInvoiceAmount?.ToString(CultureInfo.InvariantCulture) ?? ""),
                    (project.EndDate?.ToString("yyyy-MM-dd") ?? ""),
                    EscapeCsvField(project.LostReason ?? ""),
                    EscapeCsvField(project.Notes ?? "")
                ));
            }

            return csv.ToString();
        }

        public async Task<byte[]> GenerateCsvFileAsync(int? year = null)
        {
            var csvContent = await ExportProjectsToCsvAsync(year);
            return Encoding.UTF8.GetBytes(csvContent);
        }

        public async Task<(int imported, List<string> errors)> ImportProjectsFromCsvAsync(Stream csvStream)
        {
            var errors = new List<string>();
            var imported = 0;

            using var reader = new StreamReader(csvStream);

            // Skip header
            await reader.ReadLineAsync();

            var categories = await _context.Categories.ToDictionaryAsync(c => c.Name, c => c.Id);
            var leadChannels = await _context.LeadChannels.ToDictionaryAsync(lc => lc.Name, lc => lc.Id);

            int lineNumber = 1;
            while (!reader.EndOfStream)
            {
                lineNumber++;
                var line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;

                try
                {
                    var fields = ParseCsvLine(line);

                    if (fields.Length < 16)
                    {
                        errors.Add($"Line {lineNumber}: Invalid number of fields");
                        continue;
                    }

                    var project = new Project
                    {
                        Date = DateTime.Parse(fields[0]),
                        Customer = fields[1],
                        ClientType = Enum.Parse<ClientType>(fields[2]),
                        CategoryId = categories.GetValueOrDefault(fields[3], 0),
                        LeadChannelId = leadChannels.GetValueOrDefault(fields[4], 0),
                        Status = Enum.Parse<ProjectStatus>(fields[5]),
                        Amount = decimal.Parse(fields[6], CultureInfo.InvariantCulture),
                        Purchase = decimal.Parse(fields[7], CultureInfo.InvariantCulture),
                        ManualMarginPercentage = decimal.Parse(fields[8], CultureInfo.InvariantCulture), // Changed
                        Hours = decimal.Parse(fields[9], CultureInfo.InvariantCulture),
                        CafcaMarginPercentage = string.IsNullOrEmpty(fields[10]) ? null : decimal.Parse(fields[10], CultureInfo.InvariantCulture), // Changed
                        CafcaHours = string.IsNullOrEmpty(fields[11]) ? null : decimal.Parse(fields[11], CultureInfo.InvariantCulture),
                        FinalInvoiceAmount = string.IsNullOrEmpty(fields[12]) ? null : decimal.Parse(fields[12], CultureInfo.InvariantCulture),
                        EndDate = string.IsNullOrEmpty(fields[13]) ? null : DateTime.Parse(fields[13]),
                        LostReason = string.IsNullOrEmpty(fields[14]) ? null : fields[14],
                        Notes = string.IsNullOrEmpty(fields[15]) ? null : fields[15],
                        CreatedAt = DateTime.UtcNow
                    };

                    if (project.CategoryId == 0)
                    {
                        errors.Add($"Line {lineNumber}: Invalid category '{fields[3]}'");
                        continue;
                    }

                    if (project.LeadChannelId == 0)
                    {
                        errors.Add($"Line {lineNumber}: Invalid lead channel '{fields[4]}'");
                        continue;
                    }

                    _context.Projects.Add(project);
                    imported++;
                }
                catch (Exception ex)
                {
                    errors.Add($"Line {lineNumber}: {ex.Message}");
                }
            }

            if (imported > 0)
            {
                await _context.SaveChangesAsync();
            }

            return (imported, errors);
        }

        private string EscapeCsvField(string field)
        {
            if (string.IsNullOrEmpty(field))
                return "";

            if (field.Contains(",") || field.Contains("\"") || field.Contains("\n"))
            {
                return $"\"{field.Replace("\"", "\"\"")}\"";
            }

            return field;
        }

        private string[] ParseCsvLine(string line)
        {
            var fields = new List<string>();
            var currentField = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < line.Length; i++)
            {
                char c = line[i];

                if (c == '"')
                {
                    if (inQuotes && i + 1 < line.Length && line[i + 1] == '"')
                    {
                        currentField.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = !inQuotes;
                    }
                }
                else if (c == ',' && !inQuotes)
                {
                    fields.Add(currentField.ToString());
                    currentField.Clear();
                }
                else
                {
                    currentField.Append(c);
                }
            }

            fields.Add(currentField.ToString());
            return fields.ToArray();
        }
    }
}
