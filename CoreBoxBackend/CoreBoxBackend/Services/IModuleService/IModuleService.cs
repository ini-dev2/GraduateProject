using CoreBoxBackend.DTOs;
using CoreBoxBackend.Entities;

namespace CoreBoxBackend.Services.IModuleService
{
    public interface IModuleService
    {
        Task<ModuleDto> GetByIdAsync(int id, bool includeInactive = false);
        Task<IEnumerable<ModuleDto>> GetAllAsync(bool includeInactive = false);
        Task<IEnumerable<ModuleDto>> GetByUserIdAsync(int userId, bool includeInactive = false);
        Task<IEnumerable<ModuleDto>> GetPublicAsync();
        Task<ModuleDto> CreateAsync(CreateModuleDto dto, int currentUserId, string currentUserRole);
        Task<ModuleDto> UpdateAsync(int id, UpdateModuleDto dto, int currentUserId, string currentUserRole);
        Task DeleteAsync(int id, int currentUserId, string currentUserRole);
        Task<IEnumerable<Statistic>> GetStatisticsAsync(int moduleId, int currentUserId, string currentUserRole);
    }
}