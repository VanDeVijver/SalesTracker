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

            // Header
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
                    project.ManualMarginPercentage.ToString(CultureInfo.InvariantCulture),
                    project.Hours.ToString(CultureInfo.InvariantCulture),
                    (project.CafcaMarginPercentage?.ToString(CultureInfo.InvariantCulture) ?? ""),
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

            // Read and parse header
            var headerLine = await reader.ReadLineAsync();
            if (string.IsNullOrWhiteSpace(headerLine))
            {
                errors.Add("CSV file is empty");
                return (0, errors);
            }

            var headers = ParseCsvLine(headerLine);

            // Create header index map for flexible column ordering
            var headerMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            for (int i = 0; i < headers.Length; i++)
            {
                headerMap[headers[i].Trim()] = i;
            }

            // Load existing categories and lead channels
            var categories = await _context.Categories.ToDictionaryAsync(c => c.Name.ToLower(), c => c);
            var leadChannels = await _context.LeadChannels.ToDictionaryAsync(lc => lc.Name.ToLower(), lc => lc);

            int lineNumber = 1;
            while (!reader.EndOfStream)
            {
                lineNumber++;
                var line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;

                try
                {
                    var fields = ParseCsvLine(line);

                    // Parse date
                    var dateStr = GetField(fields, headerMap, "date");
                    if (!DateTime.TryParse(dateStr, out var date))
                    {
                        errors.Add($"Line {lineNumber}: Invalid date format '{dateStr}'");
                        continue;
                    }

                    // Parse customer
                    var customer = GetField(fields, headerMap, "customer");
                    if (string.IsNullOrWhiteSpace(customer))
                    {
                        errors.Add($"Line {lineNumber}: Customer is required");
                        continue;
                    }

                    // Parse client type
                    var clientTypeStr = GetField(fields, headerMap, "clientType");
                    if (!Enum.TryParse<ClientType>(clientTypeStr, true, out var clientType))
                    {
                        // Default to New if not specified or invalid
                        clientType = ClientType.New;
                    }

                    // Get or create category
                    var categoryName = GetField(fields, headerMap, "category");
                    if (string.IsNullOrWhiteSpace(categoryName))
                    {
                        errors.Add($"Line {lineNumber}: Category is required");
                        continue;
                    }

                    var categoryKey = categoryName.ToLower();
                    if (!categories.ContainsKey(categoryKey))
                    {
                        var newCategory = new Category { Name = categoryName };
                        _context.Categories.Add(newCategory);
                        await _context.SaveChangesAsync();
                        categories[categoryKey] = newCategory;
                    }

                    // Get or create lead channel
                    var leadChannelName = GetField(fields, headerMap, "leadChannel");
                    if (string.IsNullOrWhiteSpace(leadChannelName))
                    {
                        leadChannelName = "Unknown";
                    }

                    var leadChannelKey = leadChannelName.ToLower();
                    if (!leadChannels.ContainsKey(leadChannelKey))
                    {
                        var newLeadChannel = new LeadChannel { Name = leadChannelName };
                        _context.LeadChannels.Add(newLeadChannel);
                        await _context.SaveChangesAsync();
                        leadChannels[leadChannelKey] = newLeadChannel;
                    }

                    // Parse status
                    var statusStr = GetField(fields, headerMap, "status");
                    if (!Enum.TryParse<ProjectStatus>(statusStr, true, out var status))
                    {
                        status = ProjectStatus.Pending;
                    }

                    // Check for duplicate
                    var exists = await _context.Projects
                        .AnyAsync(p => p.Customer == customer && p.Date == date);

                    if (exists)
                    {
                        errors.Add($"Line {lineNumber}: Duplicate project - {customer} on {date:yyyy-MM-dd}");
                        continue;
                    }

                    // Create project
                    var project = new Project
                    {
                        Date = date,
                        Customer = customer,
                        ClientType = clientType,
                        CategoryId = categories[categoryKey].Id,
                        LeadChannelId = leadChannels[leadChannelKey].Id,
                        Status = status,
                        Amount = ParseDecimal(GetField(fields, headerMap, "amount")),
                        Purchase = ParseDecimal(GetField(fields, headerMap, "purchase")),
                        ManualMarginPercentage = ParseDecimal(GetField(fields, headerMap, "manualMargin")),
                        Hours = ParseDecimal(GetField(fields, headerMap, "hours")),
                        CafcaMarginPercentage = ParseDecimalNullable(GetField(fields, headerMap, "cafcaMargin")),
                        CafcaHours = ParseDecimalNullable(GetField(fields, headerMap, "cafcaHours")),
                        FinalInvoiceAmount = ParseDecimalNullable(GetField(fields, headerMap, "finalInvoiceAmount")),
                        EndDate = ParseDateNullable(GetField(fields, headerMap, "endDate")),
                        LostReason = GetFieldNullable(fields, headerMap, "lostReason"),
                        Notes = GetFieldNullable(fields, headerMap, "notes"),
                        CreatedAt = DateTime.UtcNow
                    };

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

        private string GetField(string[] fields, Dictionary<string, int> headerMap, string columnName)
        {
            if (headerMap.TryGetValue(columnName, out var index) && index < fields.Length)
            {
                return fields[index].Trim();
            }
            return string.Empty;
        }

        private string? GetFieldNullable(string[] fields, Dictionary<string, int> headerMap, string columnName)
        {
            var value = GetField(fields, headerMap, columnName);
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }

        private decimal ParseDecimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            // Remove common formatting characters
            value = value.Replace("€", "").Replace("$", "").Replace(" ", "").Trim();

            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                return result;

            // Try with current culture as fallback
            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.CurrentCulture, out result))
                return result;

            return 0;
        }

        private decimal? ParseDecimalNullable(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            var result = ParseDecimal(value);
            return result == 0 ? null : result;
        }

        private DateTime? ParseDateNullable(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return null;

            if (DateTime.TryParse(value, out var date))
                return date;

            return null;
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
