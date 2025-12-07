using CoreBoxBackend.DTOs;
using CoreBoxBackend.Services.IModuleService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoreBoxBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public sealed class ModulesController : ControllerBase
    {
        private readonly IModuleService _moduleService;

        public ModulesController(IModuleService moduleService)
        {
            _moduleService = moduleService;
        }

        /// <summary>
        /// Создать новый модуль (только developer/admin)
        /// </summary>
        [HttpPost("Create")]
        [Authorize(Roles = "developer,admin")]
        [ProducesResponseType(typeof(ModuleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateModuleDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var role = User.FindFirst(ClaimTypes.Role)!.Value;

            var module = await _moduleService.CreateAsync(dto, userId, role);
            return Ok(module);
        }

        /// <summary>
        /// Получить модуль по Id
        /// </summary>
        [HttpGet("GetBy{id}")]
        [ProducesResponseType(typeof(ModuleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var module = await _moduleService.GetByIdAsync(id);
            if (module == null)
                return NotFound();

            return Ok(module);
        }

        /// <summary>
        /// Получить список всех модулей (только admin)
        /// </summary>
        [HttpGet("GetAll")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(typeof(IEnumerable<ModuleDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var modules = await _moduleService.GetAllAsync();
            return Ok(modules);
        }

        /// <summary>
        /// Получить список модулей конкретного пользователя (admin/developer)
        /// </summary>
        [HttpGet("GetByUser/{userId}")]
        [Authorize(Roles = "developer,admin")]
        [ProducesResponseType(typeof(IEnumerable<ModuleDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByUser(int userId)
        {
            var modules = await _moduleService.GetByUserIdAsync(userId);
            return Ok(modules);
        }

        /// <summary>
        /// Получить список публичных модулей (доступно всем авторизованным)
        /// </summary>
        [HttpGet("GetAllPublic")]
        [ProducesResponseType(typeof(IEnumerable<ModuleDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPublic()
        {
            var modules = await _moduleService.GetPublicAsync();
            return Ok(modules);
        }

        /// <summary>
        /// Обновить модуль по Id (admin/developer)
        /// </summary>
        [HttpPut("UpdateBy{id}")]
        [Authorize(Roles = "admin,developer")]
        [ProducesResponseType(typeof(ModuleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateModuleDto dto)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var role = User.FindFirst(ClaimTypes.Role)?.Value ?? "";

            var module = await _moduleService.UpdateAsync(id, dto, currentUserId, role);
            if (module == null)
                return NotFound();

            return Ok(module);
        }

        /// <summary>
        /// Удалить модуль по Id (admin/developer)
        /// </summary>
        [HttpDelete("DeleteBy{id}")]
        [Authorize(Roles = "admin,developer")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var role = User.FindFirst(ClaimTypes.Role)?.Value ?? "";
            await _moduleService.DeleteAsync(id, currentUserId, role);
            return NoContent();
        }

        /// <summary>
        /// Получить статистику по модулю (admin/developer)
        /// </summary>
        [HttpGet("{id}/Statistics")]
        [Authorize(Roles = "admin,developer")]
        [ProducesResponseType(typeof(IEnumerable<StatisticDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStatistics(int id)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var role = User.FindFirst(ClaimTypes.Role)?.Value ?? "";

            var stats = await _moduleService.GetStatisticsAsync(id, currentUserId, role);
            if (stats == null)
                return NotFound();

            return Ok(stats);
        }
    }
}
