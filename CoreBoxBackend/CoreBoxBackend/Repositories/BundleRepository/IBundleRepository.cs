using CoreBoxBackend.Entities;

namespace CoreBoxBackend.Repositories.BundleRepository
{
    public interface IBundleRepository
    {
        Task<Bundle?> GetByIdAsync(int id);
        Task<IEnumerable<Bundle>> GetAllAsync();
        Task AddAsync(Bundle bundle);
        Task UpdateAsync(Bundle bundle);
        Task DeleteAsync(Bundle bundle);
    }
}