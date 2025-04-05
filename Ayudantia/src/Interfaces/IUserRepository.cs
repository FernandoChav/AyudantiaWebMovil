using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Dtos;
using Ayudantia.Src.Models;

namespace Ayudantia.Src.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(string firtsName);
        Task CreateUserAsync(User user, ShippingAddres? shippingAddress);
        void UpdateUserAsync(User user);
        void UpdateShippingAddressAsync(UserDto userDto);
        void DeleteUserAsync(User user, ShippingAddres shippingAddress);
    }
}