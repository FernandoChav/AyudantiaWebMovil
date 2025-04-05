using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ayudantia.Src.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string FirtsName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Thelephone { get; set; }
        // Navigation properties
        public ShippingAddres? ShippingAddres { get; set; } 
        
    }
}