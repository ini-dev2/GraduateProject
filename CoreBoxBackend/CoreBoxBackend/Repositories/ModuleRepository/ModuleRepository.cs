using CoreBoxBackend.Data;
using CoreBoxBackend.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreBoxBackend.Repositories.ModuleRepository
{
    public sealed class ModuleRepository : IModuleRepository
    {
        private readonly AppDbContext _context;
        public ModuleRepository(AppDbContext context) => _context = context;

        public async Task<Module?> GetByIdAsync(int id) =>
            await _context.Modules.Include(m => m.User)
                                  .Include(m => m.Statistics)
                                  .FirstOrDefaultAsync(m => m.Id == id);

        public async Task<IEnumerable<Module>> GetAllAsync(bool includeInactive = false) =>
            await _context.Modules.Include(m => m.User)
                                  .Where(m => includeInactive || m.IsActive)
                                  .ToListAsync();

        public async Task<IEnumerable<Module>> GetByUserIdAsync(int userId, bool includeInactive = false) =>
            await _context.Modules.Include(m => m.User)
                                  .Where(m => m.UserId == userId && (includeInactive || m.IsActive))
                                  .ToListAsync();

        public async Task<IEnumerable<Module>> GetPublicAsync() =>
            await _context.Modules.Include(m => m.User)
                                  .Where(m => m.IsActive)
                                  .ToListAsync();

        public async Task AddAsync(Module module)
        {
            await _context.Modules.AddAsync(module);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Module module)
        {
            _context.Modules.Update(module);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Module module)
        {
            module.IsActive = false;
            _context.Modules.Update(module);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Statistic>> GetStatisticsAsync(int moduleId)
        {
            var module = await _context.Modules.Include(m => m.Statistics)
                                               .FirstOrDefaultAsync(m => m.Id == moduleId);
            return module?.Statistics ?? Enumerable.Empty<Statistic>();
        }
    }
}