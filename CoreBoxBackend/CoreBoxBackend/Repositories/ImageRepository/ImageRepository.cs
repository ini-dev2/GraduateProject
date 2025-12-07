using CoreBoxBackend.Data;
using CoreBoxBackend.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreBoxBackend.Repositories.ImageRepository
{
    public sealed class ImageRepository : IImageRepository
    {
        private readonly AppDbContext _context;

        public ImageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Image image)
        {
            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();
        }

        public async Task<Image?> GetByIdAsync(int id) =>
            await _context.Images.FindAsync(id);

        public async Task<IEnumerable<Image>> GetAllAsync() =>
            await _context.Images.ToListAsync();

        public async Task DeleteAsync(Image image)
        {
            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
        }
    }
}