using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Data;
using Ayudantia.Src.Dtos;
using Ayudantia.Src.Extensions;
using Ayudantia.Src.Helpers;
using Ayudantia.Src.Mappers;
using Ayudantia.Src.Models;
using Ayudantia.Src.RequestHelpers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ayudantia.Src.Controllers
{

    public class UserController(ILogger<UserController> logger, UnitOfWork unitOfWork) : BaseController
    {
        private readonly ILogger<UserController> _logger = logger;
        private readonly UnitOfWork _unitOfWork = unitOfWork;



        // GET /user?params...
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserDto>>>> GetAll([FromQuery] UserParams userParams)
        {
            var query = _unitOfWork.UserRepository.GetUsersQueryable()
                .Filter(userParams.IsActive, userParams.RegisteredFrom, userParams.RegisteredTo)
                .Search(userParams.SearchTerm)
                .Sort(userParams.OrderBy);

            var total = await query.CountAsync();

            var users = await query
                .Skip((userParams.PageNumber - 1) * userParams.PageSize)
                .Take(userParams.PageSize)
                .ToListAsync();

            var dtos = users.Select(UserMapper.UserToUserDto).ToList();

            Response.AddPaginationHeader(new PaginationMetaData
            {
                CurrentPage = userParams.PageNumber,
                TotalPages = (int)Math.Ceiling(total / (double)userParams.PageSize),
                PageSize = userParams.PageSize,
                TotalCount = total
            });

            return Ok(new ApiResponse<IEnumerable<UserDto>>(true, "Usuarios obtenidos correctamente", dtos));
        }
        [Authorize(Roles = "Admin")]
        // GET /users/{id}
        [HttpGet("{email}")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetById(string email)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(email);
            if (user == null)
                return NotFound(new ApiResponse<string>(false, "Usuario no encontrado"));

            var dto = UserMapper.UserToUserDto(user);
            return Ok(new ApiResponse<UserDto>(true, "Usuario encontrado", dto));
        }

        [Authorize(Roles = "Admin")]
        // PUT /users/{id}/status
        [HttpPut("{email}/status")]
        public async Task<ActionResult<ApiResponse<string>>> ToggleStatus(string email, [FromBody] ToggleStatusDto dto)
        {
            var user = await _unitOfWork.UserRepository.GetUserByEmailAsync(email);
            if (user == null)
                return NotFound(new ApiResponse<string>(false, "Usuario no encontrado"));

            user.IsActive = !user.IsActive;
            user.DeactivationReason = user.IsActive ? null : dto.Reason;

            await _unitOfWork.UserRepository.UpdateUserAsync(user);
            await _unitOfWork.SaveChangeAsync();

            var message = user.IsActive ? "Usuario habilitado correctamente" : "Usuario deshabilitado correctamente";
            return Ok(new ApiResponse<string>(true, message));
        }
    }
}