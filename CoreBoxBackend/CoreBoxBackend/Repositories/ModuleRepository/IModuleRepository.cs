using CoreBoxBackend.Entities;

namespace CoreBoxBackend.Repositories.ModuleRepository
{
    public interface IModuleRepository
    {
        Task<Module?> GetByIdAsync(int id);
        Task<IEnumerable<Module>> GetAllAsync(bool includeInactive = false);
        Task<IEnumerable<Module>> GetByUserIdAsync(int userId, bool includeInactive = false);
        Task<IEnumerable<Module>> GetPublicAsync();
        Task AddAsync(Module module);
        Task UpdateAsync(Module module);
        Task DeleteAsync(Module module);
        Task<IEnumerable<Statistic>> GetStatisticsAsync(int moduleId);
    }
}