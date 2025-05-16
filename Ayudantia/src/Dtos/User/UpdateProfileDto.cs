using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ayudantia.Src.Dtos.User
{
    public class UpdateProfileDto
    {
        public string? FirtsName  { get; set; } 
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public DateOnly? BirthDate { get; set; } 
        public string? Phone { get; set; }
    }
}