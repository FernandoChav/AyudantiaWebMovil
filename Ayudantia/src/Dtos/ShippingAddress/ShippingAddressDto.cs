using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ayudantia.Src.Dtos.ShippingAddress
{
    public class ShippingAddressDto
    {
        public string Street { get; set; } = null!;
        public string Number { get; set; } = null!;
        public string Commune { get; set; } = null!;
        public string Region { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
    }
}