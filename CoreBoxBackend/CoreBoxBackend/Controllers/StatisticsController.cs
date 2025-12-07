using CoreBoxBackend.DTOs;
using CoreBoxBackend.Services.StatisticService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoreBoxBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public sealed class StatisticsController : ControllerBase
    {
        private readonly IStatisticService _statService;

        public StatisticsController(IStatisticService statService)
        {
            _statService = statService;
        }

        /// <summary>
        /// Начать сессию статистики (для текущего пользователя)
        /// </summary>
        [HttpPost("StartSession")]
        [ProducesResponseType(typeof(StatisticDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> StartSession([FromBody] StartSessionDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var stat = await _statService.StartSessionAsync(userId, dto);
            return Ok(stat);
        }

        /// <summary>
        /// Завершить сессию статистики (для текущего пользователя)
        /// </summary>
        [HttpPost("EndSession")]
        [ProducesResponseType(typeof(StatisticDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EndSession([FromBody] EndSessionDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var stat = await _statService.EndSessionAsync(userId, dto);
            return Ok(stat);
        }

        /// <summary>
        /// Получить статистику по Id (только admin)
        /// </summary>
        [HttpGet("GetBy{id}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(StatisticDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var stat = await _statService.GetByIdAsync(id);
            if (stat == null)
                return NotFound();

            return Ok(stat);
        }

        /// <summary>
        /// Получить всю статистику (только admin)
        /// </summary>
        [HttpGet("All")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(IEnumerable<StatisticDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var stats = await _statService.GetAllAsync();
            return Ok(stats);
        }

        /// <summary>
        /// Получить статистику конкретного пользователя (только admin)
        /// </summary>
        [HttpGet("ByUser/{userId}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(IEnumerable<StatisticDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var stats = await _statService.GetByUserIdAsync(userId);
            return Ok(stats);
        }

        /// <summary>
        /// Получить статистику по конкретному модулю (только admin)
        /// </summary>
        [HttpGet("ByModule/{moduleId}")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(IEnumerable<StatisticDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByModule(int moduleId)
        {
            var stats = await _statService.GetByModuleIdAsync(moduleId);
            return Ok(stats);
        }
    }
}
