using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesTracker.Core.Entities
{
    public class Category : BaseEntity
    {

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        // Navigation
        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<CategoryTarget> Targets { get; set; } = new List<CategoryTarget>();
    }
}
