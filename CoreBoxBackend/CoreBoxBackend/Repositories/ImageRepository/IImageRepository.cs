using CoreBoxBackend.Entities;

namespace CoreBoxBackend.Repositories.ImageRepository
{
    public interface IImageRepository
    {
        Task AddAsync(Image image);
        Task<Image?> GetByIdAsync(int id);
        Task<IEnumerable<Image>> GetAllAsync();
        Task DeleteAsync(Image image);
    }
}