using CoreBoxBackend.DTOs;
using CoreBoxBackend.Entities;
using CoreBoxBackend.Repositories.RoleRepository;
using CoreBoxBackend.Repositories.UserRepository;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace CoreBoxBackend.Services.UserService
{
    public sealed class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserService(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<UserDto> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id)
                       ?? throw new Exception("Пользователь не найден");

            return MapToDto(user);
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return users.Select(MapToDto);
        }

        public async Task<UserDto> CreateAsync(CreateUserDto dto)
        {
            var existing = await _userRepository.GetByEmailAsync(dto.Email);
            if (existing != null)
                throw new Exception("Пользователь с таким email уже существует");

            var role = await _roleRepository.GetByIdAsync(dto.RoleId)
                       ?? throw new Exception("Роль не найдена");

            if (role.RoleName != "developer" && role.RoleName != "user")
                throw new Exception("Админ может создавать только пользователей с ролью developer или user");

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = HashPassword(dto.Password),
                Role = role,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await _userRepository.AddAsync(user);
            return MapToDto(user);
        }

        public async Task<UserDto> UpdateAsync(int id, UpdateUserDto dto)
        {
            var user = await _userRepository.GetByIdAsync(id)
                       ?? throw new Exception("Пользователь не найден");

            if (!string.IsNullOrEmpty(dto.Username))
                user.Username = dto.Username;
            if (!string.IsNullOrEmpty(dto.Email))
                user.Email = dto.Email;
            if (dto.RoleId.HasValue)
            {
                var role = await _roleRepository.GetByIdAsync(dto.RoleId.Value)
                           ?? throw new Exception("Роль не найдена");

                if (role.RoleName != "developer" && role.RoleName != "user")
                    throw new Exception("Админ может назначать только роли developer или user");

                user.Role = role;
            }
            if (dto.IsActive.HasValue)
                user.IsActive = dto.IsActive.Value;

            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);

            return MapToDto(user);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _userRepository.GetByIdAsync(id)
                       ?? throw new Exception("Пользователь не найден");

            await _userRepository.DeleteAsync(user);
            return true;
        }

        private string HashPassword(string password)
        {
            var salt = Encoding.UTF8.GetBytes("СлучайнаяСоль123");
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32));
            return hashed;
        }

        private UserDto MapToDto(User user) => new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role.RoleName,
            IsActive = user.IsActive,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}
