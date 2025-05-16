using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Dtos;
using Ayudantia.Src.Dtos.ShippingAddress;
using Ayudantia.Src.Models;

namespace Ayudantia.Src.Mappers
{
    public static class ShippingAddressMapper
    {
        public static ShippingAddres FromDto(CreateShippingAddressDto dto, string userId)
        {
            return new ShippingAddres
            {
                Street = dto.Street,
                Number = dto.Number,
                Commune = dto.Commune,
                Region = dto.Region,
                PostalCode = dto.PostalCode,
                UserId = userId
            };
        }

        public static ShippingAddressDto ToDto(ShippingAddres shippingAddress)
        {
            return new ShippingAddressDto
            {
                Street = shippingAddress.Street,
                Number = shippingAddress.Number,
                Commune = shippingAddress.Commune,
                Region = shippingAddress.Region,
                PostalCode = shippingAddress.PostalCode
            };
        }
    }
}