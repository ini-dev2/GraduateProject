using CoreBoxBackend.Data;
using CoreBoxBackend.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreBoxBackend.Repositories.BundleRepository
{
    public sealed class BundleRepository : IBundleRepository
    {
        private readonly AppDbContext _context;

        public BundleRepository(AppDbContext context) => _context = context;

        public async Task<Bundle?> GetByIdAsync(int id)
        {
            return await _context.Bundles.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<IEnumerable<Bundle>> GetAllAsync()
        {
            return await _context.Bundles.ToListAsync();
        }

        public async Task AddAsync(Bundle bundle)
        {
            await _context.Bundles.AddAsync(bundle);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Bundle bundle)
        {
            _context.Bundles.Update(bundle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Bundle bundle)
        {
            _context.Bundles.Remove(bundle);
            await _context.SaveChangesAsync();
        }
    }
}