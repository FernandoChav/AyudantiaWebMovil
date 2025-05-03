
using Ayudantia.Src.Data;
using Ayudantia.Src.Dtos;
using Ayudantia.Src.Helpers;
using Ayudantia.Src.Interfaces;
using Ayudantia.Src.Mappers;
using Ayudantia.Src.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace Ayudantia.Src.Controllers
{

    public class AuthController(ILogger<AuthController> logger, UserManager<User> userManager, ITokenServices tokenService) : BaseController
    {
        private readonly ILogger<AuthController> _logger = logger;
        private readonly UserManager<User> _userManager = userManager;

        private readonly ITokenServices _tokenService = tokenService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto newUser)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ApiResponse<string>(false, "Datos inválidos", null, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

                var user = UserMapper.RegisterToUser(newUser);
                if (string.IsNullOrEmpty(newUser.Password) || string.IsNullOrEmpty(newUser.ConfirmPassword))
                {
                    return BadRequest(new ApiResponse<string>(false, "La contraseña y la confirmación son requeridas"));
                }

                var createUser = await _userManager.CreateAsync(user, newUser.Password);

                if (!createUser.Succeeded)
                {
                    return BadRequest(new ApiResponse<string>(false, "Error al crear el usuario", null, createUser.Errors.Select(e => e.Description).ToList()));
                }

                var roleUser = await _userManager.AddToRoleAsync(user, "User");
                if (!roleUser.Succeeded)
                {
                    return StatusCode(500, new ApiResponse<string>(false, "Error al asignar el rol", null, roleUser.Errors.Select(e => e.Description).ToList()));
                }

                var role = await _userManager.GetRolesAsync(user);
                var roleName = role.FirstOrDefault() ?? "User";

                var token = _tokenService.GenerateToken(user, roleName);
                var userDto = UserMapper.UserToAuthenticatedDto(user, token);

                return Ok(new ApiResponse<AuthenticatedUserDto>(true, "Usuario registrado exitosamente", userDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(false, "Error interno del servidor", null, new List<string> { ex.Message }));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ApiResponse<string>(false, "Datos inválidos", null, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));

                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    return Unauthorized(new ApiResponse<string>(false, "Correo o contraseña inválidos"));
                }

                if (!user.IsActive)
                {
                    return Unauthorized(new ApiResponse<string>(false, "Tu cuenta está deshabilitada. Contacta al administrador."));
                }

                var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (!result)
                {
                    return Unauthorized(new ApiResponse<string>(false, "Correo o contraseña inválidos"));
                }

                // Opcional: registrar último acceso
                user.LastAccess = DateTime.UtcNow;
                await _userManager.UpdateAsync(user);

                var roles = await _userManager.GetRolesAsync(user);
                var roleName = roles.FirstOrDefault() ?? "User";

                var token = _tokenService.GenerateToken(user, roleName);
                var userDto = UserMapper.UserToAuthenticatedDto(user, token);

                return Ok(new ApiResponse<AuthenticatedUserDto>(true, "Login exitoso", userDto));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<string>(false, "Error interno del servidor", null, new List<string> { ex.Message }));
            }
        }

    }
}