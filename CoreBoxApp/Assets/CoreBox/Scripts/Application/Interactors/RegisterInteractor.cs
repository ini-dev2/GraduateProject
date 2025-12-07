using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

namespace CoreBox
{
    internal sealed class RegisterInteractor : IRegisterUseCase
    {
        private readonly IAuthService _authService;

        [Inject]
        public RegisterInteractor(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        public async UniTask<AuthResponseDTO> ExecuteAsync(string username, string email, string password)
        {
            RegisterDTO registerDTO = new(username, email, password);
            AuthResponseDTO result = await _authService.RegisterAsync(registerDTO);

            if (result.IsSuccess)
            {
                Debug.Log("Register is success");
            }
            else
            {
                Debug.LogError(result.StatusMessage);
            }
            return result;
        }
    }
}