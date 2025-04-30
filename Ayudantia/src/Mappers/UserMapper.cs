using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Dtos;
using Ayudantia.Src.Models;

namespace Ayudantia.Src.Mappers
{
    public class UserMapper
    {

        public static User RegisterToUser(RegisterDto registerDto) =>
            new()
            {
                UserName = registerDto.Email,
                FirtsName = registerDto.FirtsName,
                LastName = registerDto.LastName,
                Thelephone = registerDto.Thelephone,
                Email = registerDto.Email,
                PhoneNumber = registerDto.Thelephone,
                ShippingAddres = new ShippingAddres
                {
                    Street = registerDto.Street ?? string.Empty,
                    Number = registerDto.Number ?? string.Empty,
                    Commune = registerDto.Commune ?? string.Empty,
                    Region = registerDto.Region ?? string.Empty,
                    PostalCode = registerDto.PostalCode ?? string.Empty
                }
            };
        public static UserDto UserToUserDto(User user, string token) =>
            new()
            {
                FirtsName = user.FirtsName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                Thelephone = user.PhoneNumber ?? string.Empty,
                Street = user.ShippingAddres?.Street,
                Number = user.ShippingAddres?.Number,
                Commune = user.ShippingAddres?.Commune,
                Region = user.ShippingAddres?.Region,
                PostalCode = user.ShippingAddres?.PostalCode,
                Token = token
            };
    }
}