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

        public static User RegisterToUser(RegisterDto dto) =>
            new()
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirtsName = dto.FirtsName,
                LastName = dto.LastName,
                PhoneNumber = dto.Thelephone,
                Thelephone = dto.Thelephone,
                RegisteredAt = DateTime.UtcNow,
                IsActive = true,
                ShippingAddres = new ShippingAddres
                {
                    Street = dto.Street ?? string.Empty,
                    Number = dto.Number ?? string.Empty,
                    Commune = dto.Commune ?? string.Empty,
                    Region = dto.Region ?? string.Empty,
                    PostalCode = dto.PostalCode ?? string.Empty
                }
            };
        public static UserDto UserToUserDto(User user) =>
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
                RegisteredAt = user.RegisteredAt,
                LastAccess = user.LastAccess,
                IsActive = user.IsActive
            };
        public static AuthenticatedUserDto UserToAuthenticatedDto(User user, string token) =>
            new()
            {
                FirtsName = user.FirtsName,
                LastName = user.LastName,
                Email = user.Email ?? string.Empty,
                Thelephone = user.PhoneNumber ?? string.Empty,
                Token = token,
                Street = user.ShippingAddres?.Street,
                Number = user.ShippingAddres?.Number,
                Commune = user.ShippingAddres?.Commune,
                Region = user.ShippingAddres?.Region,
                PostalCode = user.ShippingAddres?.PostalCode,
                RegisteredAt = user.RegisteredAt,
                LastAccess = user.LastAccess,
                IsActive = user.IsActive
            };
    }
}