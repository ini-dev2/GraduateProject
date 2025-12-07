using CoreBoxBackend.DTOs;
using CoreBoxBackend.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoreBoxBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public sealed class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService) => _userService = userService;

        /// <summary>
        /// Получить список всех пользователей (только для admin)
        /// </summary>
        [HttpGet("GetAll")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        /// <summary>
        /// Получить пользователя по Id (только для admin)
        /// </summary>
        [HttpGet("GetBy{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Создать пользователя (только для admin)
        /// </summary>
        [HttpPost("Create")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
        {
            var user = await _userService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        /// <summary>
        /// Обновить пользователя по Id (только для admin)
        /// </summary>
        [HttpPut("UpdateBy{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserDto dto)
        {
            var user = await _userService.UpdateAsync(id, dto);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        /// <summary>
        /// Удалить пользователя по Id (только для admin)
        /// </summary>
        [HttpDelete("DeleteBy{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Получить данные о текущем пользователе
        /// </summary>
        [HttpGet("Me")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMe()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null)
                return Unauthorized("Не удалось определить пользователя");

            var userId = int.Parse(userIdClaim);
            var user = await _userService.GetByIdAsync(userId);

            if (user == null)
                return NotFound();

            return Ok(user);
        }
    }
}
