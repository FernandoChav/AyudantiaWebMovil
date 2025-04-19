using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Data;
using Ayudantia.Src.Dtos;
using Ayudantia.Src.Interfaces;
using Ayudantia.Src.Mappers;
using Ayudantia.Src.Models;

using Microsoft.EntityFrameworkCore;

namespace Ayudantia.Src.Repositories
{
    public class UserRepository(StoreContext store) : IUserRepository
    {
        private readonly StoreContext _context = store;
        public async Task CreateUserAsync(User user, ShippingAddres? shippingAddress)
        {
            await _context.Users.AddAsync(user);
            if (shippingAddress != null) await _context.ShippingAddres.AddAsync(shippingAddress);
        }

        public void DeleteUserAsync(User user, ShippingAddres shippingAddress)
        {
            _context.ShippingAddres.Remove(shippingAddress);
            _context.Users.Remove(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var users = await _context.Users.Include(x => x.ShippingAddres).ToListAsync();


            return users.Select(UserMapper.MapToDTO);
        }

        public Task<UserDto> GetUserByIdAsync(string firtsName)
        {
            var user = _context.Users.Include(x => x.ShippingAddres).FirstOrDefault(x => x.FirtsName == firtsName) ?? throw new Exception("User not found");
            return Task.FromResult(UserMapper.MapToDTO(user));
        }

        public void UpdateShippingAddressAsync(UserDto userDto)
        {
            var user = _context.Users.Include(x => x.ShippingAddres).FirstOrDefault(x => x.FirtsName == userDto.FirtsName) ?? throw new Exception("User not found");

            if (user.ShippingAddres == null)
            {
                user.ShippingAddres = new ShippingAddres
                {
                    Street = userDto.Street ?? string.Empty,
                    Number = userDto.Number ?? string.Empty,
                    Commune = userDto.Commune ?? string.Empty,
                    Region = userDto.Region ?? string.Empty,
                    PostalCode = userDto.PostalCode ?? string.Empty
                };
            }
            else
            {
                user.ShippingAddres.Street = userDto.Street ?? string.Empty;
                user.ShippingAddres.Number = userDto.Number ?? string.Empty;
                user.ShippingAddres.Commune = userDto.Commune ?? string.Empty;
                user.ShippingAddres.Region = userDto.Region ?? string.Empty;
                user.ShippingAddres.PostalCode = userDto.PostalCode ?? string.Empty;
            }
            _context.ShippingAddres.Update(user.ShippingAddres);
            _context.Users.Update(user);
        }


        public void UpdateUserAsync(User user)
        {
            var existingUser = _context.Users.FirstOrDefault(x => x.FirtsName == user.FirtsName) ?? throw new Exception("User not found");
            if (existingUser != null)
            {
                existingUser.LastName = user.LastName;
                existingUser.Thelephone = user.Thelephone;
                existingUser.Email = user.Email;
                _context.Users.Update(existingUser);
            }
            else
            {
                throw new Exception("User not found");
            }
        }
    }
}