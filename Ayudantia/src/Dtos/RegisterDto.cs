using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ayudantia.Src.Dtos
{
    public class RegisterDto
    {
        public required string FirtsName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Thelephone { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
        public string? Street { get; set; }
        public string? Number { get; set; }
        public string? Commune { get; set; }
        public string? Region { get; set; }
        public string? PostalCode { get; set; }
    }
}