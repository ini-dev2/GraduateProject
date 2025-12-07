using Cysharp.Threading.Tasks;

namespace CoreBox
{
    /// <summary>
    /// Rules for login logic
    /// </summary>
    internal interface ILoginUseCase
    {
        public UniTask<AuthResponseDTO> ExecuteAsync(string email, string password);
    }
}