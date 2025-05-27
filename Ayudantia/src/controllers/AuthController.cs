
using Ayudantia.Src.Data;
using Ayudantia.Src.Dtos;
using Ayudantia.Src.Dtos.Auth;
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
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(new ApiResponse<string>(
                        false,
                        "Datos inválidos",
                        null,
                        errors));
                }

                if (string.IsNullOrWhiteSpace(newUser.Password) || string.IsNullOrWhiteSpace(newUser.ConfirmPassword))
                    return BadRequest(new ApiResponse<string>(false, "La contraseña y la confirmación son requeridas"));

                if (newUser.Password != newUser.ConfirmPassword)
                    return BadRequest(new ApiResponse<string>(false, "Las contraseñas no coinciden"));

                var existingUser = await _userManager.FindByEmailAsync(newUser.Email);
                if (existingUser != null)
                    return Conflict(new ApiResponse<string>(false, "Ya existe una cuenta con este correo electrónico"));

                var user = UserMapper.RegisterToUser(newUser);
                var createUser = await _userManager.CreateAsync(user, newUser.Password);

                if (!createUser.Succeeded)
                {
                    var identityErrors = createUser.Errors.Select(e => e.Description).ToList();
                    return BadRequest(new ApiResponse<string>(
                        false,
                        "Error al crear el usuario",
                        null,
                        identityErrors));
                }

                var roleResult = await _userManager.AddToRoleAsync(user, "User");
                if (!roleResult.Succeeded)
                {
                    var roleErrors = roleResult.Errors.Select(e => e.Description).ToList();
                    return StatusCode(500, new ApiResponse<string>(
                        false,
                        "Error al asignar el rol al usuario",
                        null,
                        roleErrors));
                }

                var userDto = UserMapper.UserToNewUserDto(user);
                return Ok(new ApiResponse<NewUserDto>(true, "Usuario registrado exitosamente", userDto));
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