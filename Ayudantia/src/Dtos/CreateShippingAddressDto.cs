using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ayudantia.Src.Dtos
{
    public class CreateShippingAddressDto
    {
        public required string Street { get; set; }
        public required string Number { get; set; }
        public required string Commune { get; set; }
        public required string Region { get; set; }
        public required string PostalCode { get; set; }
    }
}