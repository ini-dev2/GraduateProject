using CoreBoxBackend.DTOs;
using CoreBoxBackend.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace CoreBoxBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        /// <param name="dto">Данные для регистрации</param>
        /// <returns>Информация о пользователе</returns>
        [HttpPost("Register")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            try
            {
                var result = await _authService.RegisterAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new ErrorResponseDto { Error = ex.Message });
            }
        }

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="dto">Данные для входа</param>
        /// <returns>JWT токен</returns>
        [HttpPost("Login")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponseDto), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            try
            {
                var result = await _authService.LoginAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return Unauthorized(new ErrorResponseDto { Error = ex.Message });
            }
        }
    }
}
