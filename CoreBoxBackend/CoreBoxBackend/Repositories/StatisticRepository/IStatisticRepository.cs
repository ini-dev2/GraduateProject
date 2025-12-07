using CoreBoxBackend.Entities;

namespace CoreBoxBackend.Repositories.StatisticRepository
{
    public interface IStatisticRepository
    {
        Task<Statistic> AddAsync(Statistic statistic);
        Task<Statistic?> GetByIdAsync(int id);
        Task<IEnumerable<Statistic>> GetAllAsync();
        Task<IEnumerable<Statistic>> GetByUserIdAsync(int userId);
        Task<IEnumerable<Statistic>> GetByModuleIdAsync(int moduleId);
        Task UpdateAsync(Statistic statistic);
    }
}