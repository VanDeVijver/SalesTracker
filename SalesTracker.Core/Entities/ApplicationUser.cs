using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesTracker.Core.Entities
{

        public class ApplicationUser : IdentityUser
        {
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
            public bool IsActive { get; set; } = true;

            // Navigation properties
            public string FullName => $"{FirstName} {LastName}";
        }
    }


