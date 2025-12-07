using CoreBoxBackend.Data;
using CoreBoxBackend.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreBoxBackend.Repositories.StatisticRepository
{
    public sealed class StatisticRepository : IStatisticRepository
    {
        private readonly AppDbContext _context;

        public StatisticRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Statistic> AddAsync(Statistic statistic)
        {
            await _context.Statistics.AddAsync(statistic);
            await _context.SaveChangesAsync();
            return statistic;
        }

        public async Task<Statistic?> GetByIdAsync(int id)
        {
            return await _context.Statistics
                .Include(s => s.User)
                .Include(s => s.Module)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<IEnumerable<Statistic>> GetAllAsync()
        {
            return await _context.Statistics
                .Include(s => s.User)
                .Include(s => s.Module)
                .ToListAsync();
        }

        public async Task<IEnumerable<Statistic>> GetByUserIdAsync(int userId)
        {
            return await _context.Statistics
                .Include(s => s.Module)
                .Where(s => s.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Statistic>> GetByModuleIdAsync(int moduleId)
        {
            return await _context.Statistics
                .Include(s => s.User)
                .Where(s => s.ModuleId == moduleId)
                .ToListAsync();
        }

        public async Task UpdateAsync(Statistic statistic)
        {
            _context.Statistics.Update(statistic);
            await _context.SaveChangesAsync();
        }
    }
}