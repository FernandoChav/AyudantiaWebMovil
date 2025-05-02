using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ayudantia.Src.RequestHelpers
{
    public class UserParams : PaginationParams
    {
        public bool? IsActive { get; set; } // Filter by active/inactive status
        public DateTime? RegisteredFrom { get; set; } // Filter by registration date start
        public DateTime? RegisteredTo { get; set; }   // Filter by registration date end
        public string? SearchTerm { get; set; } // Search by full name or email
    }
}