using CoreBoxBackend.Data;
using CoreBoxBackend.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoreBoxBackend.Repositories.RoleRepository
{
    public sealed class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;
        public RoleRepository(AppDbContext context) => _context = context;

        public async Task<Role?> GetByIdAsync(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Role?> GetByNameAsync(string roleName)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == roleName);
        }
    }
}