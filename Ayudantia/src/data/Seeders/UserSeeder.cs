using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Dtos;
using Ayudantia.Src.Mappers;
using Ayudantia.Src.Models;

using Bogus;

using Microsoft.AspNetCore.Identity;

namespace Ayudantia.Src.Data.Seeders
{
    public class UserSeeder
    {
        public static List<RegisterDto> GenerateUserDtos(int quantity = 10)
        {
            var users = new Faker<RegisterDto>()
                .RuleFor(u => u.FirtsName, f => f.Person.FirstName)
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Password, f => f.Internet.Password(8, false, "[A-Za-z0-9]", "1a")) // <-- AQUI
                .RuleFor(u => u.LastName, f => f.Person.LastName)
                .RuleFor(u => u.Thelephone, f => f.Phone.PhoneNumber())
                .RuleFor(u => u.ConfirmPassword, (f, u) => u.Password)
                .RuleFor(u => u.Street, f => f.Address.StreetName())
                .RuleFor(u => u.Number, f => f.Address.BuildingNumber())
                .RuleFor(u => u.Commune, f => f.Address.City())
                .RuleFor(u => u.Region, f => f.Address.State())
                .RuleFor(u => u.PostalCode, f => f.Address.ZipCode())
                .Generate(quantity);

            return users;
        }


        public static async Task CreateUsers(UserManager<User> userManager, List<RegisterDto> userDtos)
        {
            foreach (var userDto in userDtos)
            {
                var user = UserMapper.RegisterToUser(userDto);
                user.UserName = userDto.Email;
                user.Email = userDto.Email;

                var result = await userManager.CreateAsync(user, userDto.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
                else
                {
                    throw new Exception($"Error creating user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}