using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

namespace Ayudantia.Src.Models
{
    public class User : IdentityUser
    {
        public required string FirtsName { get; set; }
        public required string LastName { get; set; }
        public required string Thelephone { get; set; }
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow; // Registration date
        public DateTime? LastAccess { get; set; } // Last login timestamp
        public bool IsActive { get; set; } = true; // Whether the user can log in
        public string? DeactivationReason { get; set; } // Admin reason for disabling the account

        // Navigation properties
        public ShippingAddres? ShippingAddres { get; set; }



    }
}