using Cysharp.Threading.Tasks;

namespace CoreBox
{
    internal interface IAuthService
    {
        public UniTask<AuthResponseDTO> LoginAsync(LoginDTO loginDTO);
        public UniTask<AuthResponseDTO> RegisterAsync(RegisterDTO registerDTO);
    }
}