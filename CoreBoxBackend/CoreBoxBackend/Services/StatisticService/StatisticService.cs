using CoreBoxBackend.DTOs;
using CoreBoxBackend.Entities;
using CoreBoxBackend.Repositories.ModuleRepository;
using CoreBoxBackend.Repositories.StatisticRepository;
using CoreBoxBackend.Repositories.UserRepository;

namespace CoreBoxBackend.Services.StatisticService
{
    public sealed class StatisticService : IStatisticService
    {
        private readonly IStatisticRepository _statRepo;
        private readonly IUserRepository _userRepo;
        private readonly IModuleRepository _moduleRepo;

        public StatisticService(IStatisticRepository statRepo, IUserRepository userRepo, IModuleRepository moduleRepo)
        {
            _statRepo = statRepo;
            _userRepo = userRepo;
            _moduleRepo = moduleRepo;
        }

        public async Task<StatisticDto> StartSessionAsync(int userId, StartSessionDto dto)
        {
            var user = await _userRepo.GetByIdAsync(userId)
                       ?? throw new Exception("Пользователь не найден");

            var module = await _moduleRepo.GetByIdAsync(dto.ModuleId)
                        ?? throw new Exception("Модуль не найден");

            var statistic = new Statistic
            {
                UserId = user.Id,
                User = user,
                ModuleId = module.Id,
                Module = module,
                StartedAt = DateTime.UtcNow
            };

            await _statRepo.AddAsync(statistic);
            return MapToDto(statistic);
        }

        public async Task<StatisticDto> EndSessionAsync(int userId, EndSessionDto dto)
        {
            var stat = await _statRepo.GetByIdAsync(dto.SessionId)
                       ?? throw new Exception("Сессия не найдена");

            if (stat.UserId != userId)
                throw new Exception("Нельзя завершить чужую сессию");

            if (stat.EndedAt != default)
                throw new Exception("Сессия уже завершена");

            stat.EndedAt = DateTime.UtcNow;
            await _statRepo.UpdateAsync(stat);

            return MapToDto(stat);
        }

        public async Task<StatisticDto> GetByIdAsync(int id)
        {
            var stat = await _statRepo.GetByIdAsync(id)
                       ?? throw new Exception("Сессия не найдена");

            return MapToDto(stat);
        }

        public async Task<IEnumerable<StatisticDto>> GetAllAsync()
        {
            var stats = await _statRepo.GetAllAsync();
            return stats.Select(MapToDto);
        }

        public async Task<IEnumerable<StatisticDto>> GetByUserIdAsync(int userId)
        {
            var stats = await _statRepo.GetByUserIdAsync(userId);
            return stats.Select(MapToDto);
        }

        public async Task<IEnumerable<StatisticDto>> GetByModuleIdAsync(int moduleId)
        {
            var stats = await _statRepo.GetByModuleIdAsync(moduleId);
            return stats.Select(MapToDto);
        }

        private StatisticDto MapToDto(Statistic stat) => new StatisticDto
        {
            Id = stat.Id,
            UserId = stat.UserId,
            ModuleId = stat.ModuleId,
            StartedAt = stat.StartedAt,
            EndedAt = stat.EndedAt
        };
    }
}