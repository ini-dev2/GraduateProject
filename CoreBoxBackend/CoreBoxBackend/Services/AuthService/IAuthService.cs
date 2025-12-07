using CoreBoxBackend.DTOs;

namespace CoreBoxBackend.Services.AuthService
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto dto);
    }
}