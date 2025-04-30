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

        // Navigation properties
        public ShippingAddres? ShippingAddres { get; set; }

    }
}