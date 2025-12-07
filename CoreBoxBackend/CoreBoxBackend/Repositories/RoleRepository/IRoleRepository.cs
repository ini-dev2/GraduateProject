using CoreBoxBackend.Entities;

namespace CoreBoxBackend.Repositories.RoleRepository
{
    public interface IRoleRepository
    {
        Task<Role?> GetByIdAsync(int id);
        Task<Role?> GetByNameAsync(string roleName);
    }
}