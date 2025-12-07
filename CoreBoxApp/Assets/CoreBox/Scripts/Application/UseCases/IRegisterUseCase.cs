using Cysharp.Threading.Tasks;

namespace CoreBox
{
    internal interface IRegisterUseCase
    {
        public UniTask<AuthResponseDTO> ExecuteAsync(string username, string email, string password);
    }
}