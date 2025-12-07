using CoreBoxBackend.DTOs;
using CoreBoxBackend.Entities;
using CoreBoxBackend.Repositories.ModuleRepository;
using CoreBoxBackend.Repositories.UserRepository;

namespace CoreBoxBackend.Services.IModuleService
{
    public sealed class ModuleService : IModuleService
    {
        private readonly IModuleRepository _moduleRepo;
        private readonly IUserRepository _userRepo;

        public ModuleService(IModuleRepository moduleRepo, IUserRepository userRepo)
        {
            _moduleRepo = moduleRepo;
            _userRepo = userRepo;
        }

        public async Task<ModuleDto> GetByIdAsync(int id, bool includeInactive = false)
        {
            var module = await _moduleRepo.GetByIdAsync(id)
                         ?? throw new Exception("Модуль не найден");

            if (!module.IsActive && !includeInactive)
                throw new Exception("Модуль неактивен");

            return MapToDto(module);
        }

        public async Task<IEnumerable<ModuleDto>> GetAllAsync(bool includeInactive = false) =>
            (await _moduleRepo.GetAllAsync(includeInactive)).Select(MapToDto);

        public async Task<IEnumerable<ModuleDto>> GetByUserIdAsync(int userId, bool includeInactive = false) =>
            (await _moduleRepo.GetByUserIdAsync(userId, includeInactive)).Select(MapToDto);

        public async Task<IEnumerable<ModuleDto>> GetPublicAsync() =>
            (await _moduleRepo.GetPublicAsync()).Select(MapToDto);

        public async Task<ModuleDto> CreateAsync(CreateModuleDto dto, int currentUserId, string currentUserRole)
        {
            if (currentUserRole != "developer" && currentUserRole != "admin")
                throw new Exception("Только разработчик или админ может создавать модуль");

            var user = await _userRepo.GetByIdAsync(currentUserId)
                       ?? throw new Exception("Пользователь не найден");

            var module = new Module
            {
                Name = dto.Name,
                Description = dto.Description,
                UserId = user.Id,
                User = user,
                BundleUrl = dto.BundleUrl,
                ImageUrl = dto.ImageUrl,
                Version = dto.Version,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _moduleRepo.AddAsync(module);
            return MapToDto(module);
        }

        public async Task<ModuleDto> UpdateAsync(int id, UpdateModuleDto dto, int currentUserId, string currentUserRole)
        {
            var module = await _moduleRepo.GetByIdAsync(id)
                         ?? throw new Exception("Модуль не найден");

            if (module.UserId != currentUserId && currentUserRole != "admin")
                throw new Exception("Нет прав на обновление");

            if (!string.IsNullOrEmpty(dto.Name))
                module.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.Description))
                module.Description = dto.Description;
            if (!string.IsNullOrEmpty(dto.BundleUrl))
                module.BundleUrl = dto.BundleUrl;
            if (!string.IsNullOrEmpty(dto.Version))
                module.Version = dto.Version;
            if (dto.IsActive.HasValue)
                module.IsActive = dto.IsActive.Value;

            module.UpdatedAt = DateTime.UtcNow;
            await _moduleRepo.UpdateAsync(module);
            return MapToDto(module);
        }

        public async Task DeleteAsync(int id, int currentUserId, string currentUserRole)
        {
            var module = await _moduleRepo.GetByIdAsync(id)
                         ?? throw new Exception("Модуль не найден");

            if (module.UserId != currentUserId && currentUserRole != "admin")
                throw new Exception("Нет прав на удаление");

            await _moduleRepo.DeleteAsync(module);
        }

        public async Task<IEnumerable<Statistic>> GetStatisticsAsync(int moduleId, int currentUserId, string currentUserRole)
        {
            var module = await _moduleRepo.GetByIdAsync(moduleId)
                         ?? throw new Exception("Модуль не найден");

            if (module.UserId != currentUserId && currentUserRole != "admin")
                throw new Exception("Нет прав на просмотр статистики");

            return await _moduleRepo.GetStatisticsAsync(moduleId);
        }

        private ModuleDto MapToDto(Module module) => new ModuleDto
        {
            Id = module.Id,
            Name = module.Name,
            Description = module.Description,
            UserId = module.UserId,
            Username = module.User.Username,
            BundleUrl = module.BundleUrl,
            ImageUrl = module.ImageUrl,
            Version = module.Version,
            IsActive = module.IsActive,
            CreatedAt = module.CreatedAt,
            UpdatedAt = module.UpdatedAt
        };
    }
}