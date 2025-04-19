using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Data;
using Ayudantia.Src.Dtos;
using Ayudantia.Src.Models;

using Microsoft.AspNetCore.Mvc;

namespace Ayudantia.Src.Controllers
{
    public class UserController(ILogger<UserController> logger, UnitOfWork unitOfWork) : BaseController
    {
        private readonly ILogger<UserController> _logger = logger;
        private readonly UnitOfWork _context = unitOfWork;

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _context.UserRepository.GetAllUsersAsync();
            return Ok(users);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto userDto)
        {
            if (userDto.ConfirmPassword != userDto.Password)
            {
                return BadRequest("Passwords do not match");
            }
            var user = new User
            {
                FirtsName = userDto.FirtsName,
                LastName = userDto.LastName,
                Email = userDto.Email,
                Password = userDto.Password,
                Thelephone = userDto.Thelephone,
                ShippingAddres = new ShippingAddres
                {
                    Street = userDto.Street ?? string.Empty,
                    Number = userDto.Number ?? string.Empty,
                    Commune = userDto.Commune ?? string.Empty,
                    Region = userDto.Region ?? string.Empty,
                    PostalCode = userDto.PostalCode ?? string.Empty
                }
            };
            await _context.UserRepository.CreateUserAsync(user, user.ShippingAddres);
            await _context.SaveChangeAsync();
            return Ok(user);
        }
    }
}