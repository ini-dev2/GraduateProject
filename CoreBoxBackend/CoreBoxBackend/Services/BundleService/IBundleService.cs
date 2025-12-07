using CoreBoxBackend.DTOs;

namespace CoreBoxBackend.Services.BundleService
{
    public interface IBundleService
    {
        Task<BundleDto> CreateAsync(CreateBundleDto dto, string currentUserRole);
        Task<BundleDto> UpdateAsync(int id, UpdateBundleDto dto, string currentUserRole);
        Task<bool> DeleteAsync(int id, string currentUserRole);
        Task<BundleDto> GetByIdAsync(int id);
        Task<IEnumerable<BundleDto>> GetAllAsync();
    }
}