using CoreBoxBackend.DTOs;
using CoreBoxBackend.Services.ImageService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreBoxBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin, developer")]
    public sealed class ImagesController : ControllerBase
    {
        private readonly IImageService _imageService;

        public ImagesController(IImageService imageService)
        {
            _imageService = imageService;
        }

        /// <summary>
        /// Создать новое изображение (admin/developer)
        /// </summary>
        [HttpPost("Create")]
        [ProducesResponseType(typeof(ImageDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromForm] CreateImageDto dto)
        {
            var image = await _imageService.CreateAsync(dto);
            return Ok(image);
        }

        /// <summary>
        /// Получить изображение по Id
        /// </summary>
        [HttpGet("GetBy{id}")]
        [ProducesResponseType(typeof(ImageDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            var image = await _imageService.GetByIdAsync(id);
            if (image == null)
                return NotFound();

            return Ok(image);
        }

        /// <summary>
        /// Получить список всех изображений
        /// </summary>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(IEnumerable<ImageDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var images = await _imageService.GetAllAsync();
            return Ok(images);
        }

        /// <summary>
        /// Удалить изображение по Id (admin/developer)
        /// </summary>
        [HttpDelete("DeleteBy{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _imageService.DeleteAsync(id);
            return NoContent();
        }
    }
}
