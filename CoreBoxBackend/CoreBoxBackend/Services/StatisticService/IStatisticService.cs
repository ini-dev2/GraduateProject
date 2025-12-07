using CoreBoxBackend.DTOs;

namespace CoreBoxBackend.Services.StatisticService
{
    public interface IStatisticService
    {
        Task<StatisticDto> StartSessionAsync(int userId, StartSessionDto dto);
        Task<StatisticDto> EndSessionAsync(int userId, EndSessionDto dto);
        Task<StatisticDto> GetByIdAsync(int id);
        Task<IEnumerable<StatisticDto>> GetAllAsync();
        Task<IEnumerable<StatisticDto>> GetByUserIdAsync(int userId);
        Task<IEnumerable<StatisticDto>> GetByModuleIdAsync(int moduleId);
    }
}