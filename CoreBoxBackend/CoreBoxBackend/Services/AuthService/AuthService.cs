using CoreBoxBackend.DTOs;
using CoreBoxBackend.Entities;
using CoreBoxBackend.Helpers;
using CoreBoxBackend.Repositories.RoleRepository;
using CoreBoxBackend.Repositories.UserRepository;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace CoreBoxBackend.Services.AuthService
{
    public sealed class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly JwtHelper _jwtHelper;

        public AuthService(IUserRepository userRepository, IRoleRepository roleRepository, JwtHelper jwtHelper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _jwtHelper = jwtHelper;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);
            if (user == null || !VerifyPassword(dto.Password, user.PasswordHash))
                throw new Exception("Неверный email или пароль");

            return new AuthResponseDto
            {
                Token = _jwtHelper.GenerateToken(user),
                Username = user.Username,
                Role = user.Role.RoleName
            };
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto)
        {
            if (String.IsNullOrEmpty(dto.Username) || String.IsNullOrEmpty(dto.Email) || String.IsNullOrEmpty(dto.Password))
                throw new Exception("Пустые поля");

            var existing = await _userRepository.GetByEmailAsync(dto.Email);
            if (existing != null)
                throw new Exception("Пользователь с таким email уже существует");

            var role = await _roleRepository.GetByNameAsync("user")
                       ?? throw new Exception("Роль User не найдена");

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

            return new AuthResponseDto
            {
                Token = _jwtHelper.GenerateToken(user),
                Username = user.Username,
                Role = role.RoleName
            };
        }

        private string HashPassword(string password)
        {
            var salt = Encoding.UTF8.GetBytes("RandomSil213");
            var hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32));
            return hashed;
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            return HashPassword(password) == storedHash;
        }
    }
}