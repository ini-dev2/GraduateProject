using CoreBoxBackend.DTOs;
using CoreBoxBackend.Entities;
using CoreBoxBackend.Repositories.BundleRepository;

namespace CoreBoxBackend.Services.BundleService
{
    public sealed class BundleService : IBundleService
    {
        private readonly IBundleRepository _bundleRepo;

        public BundleService(IBundleRepository bundleRepo)
        {
            _bundleRepo = bundleRepo;
        }

        public async Task<BundleDto> CreateAsync(CreateBundleDto dto, string currentUserRole)
        {
            if (currentUserRole != "developer" && currentUserRole != "admin")
                throw new Exception("Только developer или admin может создавать бандл");

            var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "UploadedBundles");
            if (!Directory.Exists(uploadsDir))
                Directory.CreateDirectory(uploadsDir);

            var fileName = $"{Guid.NewGuid()}_{dto.File.FileName}";
            var filePath = Path.Combine(uploadsDir, fileName);

            await using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            var bundle = new Bundle
            {
                Name = dto.Name,
                BundleUrl = $"/UploadedBundles/{fileName}",
                Version = dto.Version,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _bundleRepo.AddAsync(bundle);
            return MapToDto(bundle);
        }

        public async Task<BundleDto> UpdateAsync(int id, UpdateBundleDto dto, string currentUserRole)
        {
            if (currentUserRole != "developer" && currentUserRole != "admin")
                throw new Exception("Только developer или admin может обновлять бандл");

            var bundle = await _bundleRepo.GetByIdAsync(id)
                         ?? throw new Exception("Бандл не найден");

            if (!string.IsNullOrEmpty(dto.Name))
                bundle.Name = dto.Name;
            if (!string.IsNullOrEmpty(dto.BundleUrl))
                bundle.BundleUrl = dto.BundleUrl;
            if (!string.IsNullOrEmpty(dto.Version))
                bundle.Version = dto.Version;

            bundle.UpdatedAt = DateTime.UtcNow;
            await _bundleRepo.UpdateAsync(bundle);

            return MapToDto(bundle);
        }

        public async Task<bool> DeleteAsync(int id, string currentUserRole)
        {
            if (currentUserRole != "developer" && currentUserRole != "admin")
                throw new Exception("Только developer или admin может удалять бандл");

            var bundle = await _bundleRepo.GetByIdAsync(id)
                         ?? throw new Exception("Бандл не найден");

            await _bundleRepo.DeleteAsync(bundle);
            return true;
        }

        public async Task<BundleDto> GetByIdAsync(int id)
        {
            var bundle = await _bundleRepo.GetByIdAsync(id)
                         ?? throw new Exception("Бандл не найден");

            return MapToDto(bundle);
        }

        public async Task<IEnumerable<BundleDto>> GetAllAsync()
        {
            var bundles = await _bundleRepo.GetAllAsync();
            return bundles.Select(MapToDto);
        }

        private static BundleDto MapToDto(Bundle bundle) => new BundleDto
        {
            Id = bundle.Id,
            Name = bundle.Name,
            BundleUrl = bundle.BundleUrl,
            Version = bundle.Version,
            CreatedAt = bundle.CreatedAt,
            UpdatedAt = bundle.UpdatedAt
        };
    }
}