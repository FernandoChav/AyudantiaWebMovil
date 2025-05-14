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
                .RuleFor(u => u.Password, f => "User" + f.Random.Number(1000, 9999).ToString())
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
            var admin = new User
            {
                UserName = "ignacio.mancilla@gmail.com",
                Email = "ignacio.mancilla@gmail.com",
                FirtsName = "Ignacio",
                LastName = "Mancilla",
                Thelephone = "999999999",
                RegisteredAt = DateTime.UtcNow,
                IsActive = true,
                ShippingAddres = new ShippingAddres
                {
                    Street = "Central",
                    Number = "1000",
                    Commune = "Santiago",
                    Region = "RM",
                    PostalCode = "0000000"
                }
            };

            var existingAdmin = await userManager.FindByEmailAsync(admin.Email);
            if (existingAdmin == null)
            {
                var result = await userManager.CreateAsync(admin, "Pa$$word2025");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
                else
                {
                    throw new Exception($"Error creating required admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
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