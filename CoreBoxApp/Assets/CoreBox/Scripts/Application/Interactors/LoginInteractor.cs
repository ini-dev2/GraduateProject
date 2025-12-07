using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Zenject;

namespace CoreBox
{
    internal sealed class LoginInteractor : ILoginUseCase
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;

        [Inject]
        public LoginInteractor(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
            _tokenProvider = tokenProvider ?? throw new ArgumentNullException(nameof(tokenProvider));
        }

        /// <summary>
        /// Method which we using in Presentation layer
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async UniTask<AuthResponseDTO> ExecuteAsync(string email, string password)
        {
            LoginDTO loginDTO = new(email, password);
            AuthResponseDTO result = await _authService.LoginAsync(loginDTO);

            if (result.IsSuccess)
            {
                _tokenProvider.SaveToken(result.Token);
            }
            else
            {
                Debug.LogError(result.StatusMessage);
            }
            return result;
        }
    }
}