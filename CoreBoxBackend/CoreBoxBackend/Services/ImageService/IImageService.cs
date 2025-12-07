using CoreBoxBackend.DTOs;

namespace CoreBoxBackend.Services.ImageService
{
    public interface IImageService
    {
        Task<ImageDto> CreateAsync(CreateImageDto dto);
        Task<ImageDto> GetByIdAsync(int id);
        Task<IEnumerable<ImageDto>> GetAllAsync();
        Task DeleteAsync(int id);
    }
}