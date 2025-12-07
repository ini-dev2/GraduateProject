using CoreBoxBackend.DTOs;
using CoreBoxBackend.Services.BundleService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CoreBoxBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "developer,admin")]
    public sealed class BundlesController : ControllerBase
    {
        private readonly IBundleService _bundleService;
        public BundlesController(IBundleService bundleService) => _bundleService = bundleService;

        private string CurrentUserRole => User.FindFirst(ClaimTypes.Role)?.Value ?? "";

        /// <summary>
        /// Получить все бандлы
        /// </summary>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(IEnumerable<BundleDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var bundles = await _bundleService.GetAllAsync();
            return Ok(bundles);
        }

        /// <summary>
        /// Получить бандл по Id
        /// </summary>
        [HttpGet("GetBy{id}")]
        [ProducesResponseType(typeof(BundleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var bundle = await _bundleService.GetByIdAsync(id);
            if (bundle == null)
                return NotFound();

            return Ok(bundle);
        }

        /// <summary>
        /// Создать новый бандл (developer/admin)
        /// </summary>
        [HttpPost("Create")]
        [ProducesResponseType(typeof(BundleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromForm] CreateBundleDto dto)
        {
            var bundle = await _bundleService.CreateAsync(dto, CurrentUserRole);
            return Ok(bundle);
        }

        /// <summary>
        /// Обновить бандл по Id (developer/admin)
        /// </summary>
        [HttpPut("UpdateBy{id}")]
        [ProducesResponseType(typeof(BundleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBundleDto dto)
        {
            var bundle = await _bundleService.UpdateAsync(id, dto, CurrentUserRole);
            return Ok(bundle);
        }

        /// <summary>
        /// Удалить бандл по Id (developer/admin)
        /// </summary>
        [HttpDelete("DeleteBy{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _bundleService.DeleteAsync(id, CurrentUserRole);
            return NoContent();
        }
    }
}
