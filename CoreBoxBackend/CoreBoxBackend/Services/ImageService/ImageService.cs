using CoreBoxBackend.DTOs;
using CoreBoxBackend.Entities;
using CoreBoxBackend.Repositories.ImageRepository;

namespace CoreBoxBackend.Services.ImageService
{
    public sealed class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepo;

        public ImageService(IImageRepository imageRepo)
        {
            _imageRepo = imageRepo;
        }

        public async Task<ImageDto> CreateAsync(CreateImageDto dto)
        {
            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "UploadedImages");
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            var fileName = $"{Guid.NewGuid()}_{dto.File.FileName}";
            var filePath = Path.Combine(uploadsDir, fileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            var image = new Image
            {
                Name = dto.Name,
                ImageUrl = $"/UploadedImages/{fileName}",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _imageRepo.AddAsync(image);

            return MapToDto(image);
        }

        public async Task<ImageDto> GetByIdAsync(int id)
        {
            var image = await _imageRepo.GetByIdAsync(id)
                       ?? throw new Exception("Изображение не найдено");
            return MapToDto(image);
        }

        public async Task<IEnumerable<ImageDto>> GetAllAsync()
        {
            var images = await _imageRepo.GetAllAsync();
            return images.Select(MapToDto);
        }

        public async Task DeleteAsync(int id)
        {
            var image = await _imageRepo.GetByIdAsync(id)
                       ?? throw new Exception("Изображение не найдено");

            await _imageRepo.DeleteAsync(image);
        }

        private ImageDto MapToDto(Image image) => new ImageDto
        {
            Id = image.Id,
            Name = image.Name,
            ImageUrl = image.ImageUrl,
            CreatedAt = image.CreatedAt,
            UpdatedAt = image.UpdatedAt
        };
    }
}