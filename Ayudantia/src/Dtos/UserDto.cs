using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ayudantia.Src.Dtos
{
    public class UserDto
    {
        public required string FirtsName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string Thelephone { get; set; }
        public required string Street { get; set; }
        public required string Number { get; set; }
        public required string Commune {get; set;}
        public required string Region { get; set; }
        public required string PostalCode { get; set; }
    }
}