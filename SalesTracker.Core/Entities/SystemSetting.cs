using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesTracker.Core.Entities
{
    public class SystemSetting : BaseEntity
    {
        [Required]
        [MaxLength(100)]
        public string Key { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Value { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; }
    }
}
