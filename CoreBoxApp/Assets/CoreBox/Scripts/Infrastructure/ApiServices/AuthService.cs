using Cysharp.Threading.Tasks;
using RestBuilder;
using System;
using Zenject;

namespace CoreBox
{
    /// <summary>
    /// Auhtorization service for work with back API
    /// Token provider for other services
    /// </summary>
    internal sealed class AuthService : IAuthService
    {
        private readonly IApiService _apiService;
        private readonly string loginEndPoint;
        private readonly string registerEndPoint;

        [Inject]
        public AuthService(IApiService apiService, string loginEndPoint, string registerEndPoint)
        {
            _apiService = apiService ?? throw new ArgumentNullException(nameof(apiService));
            this.loginEndPoint = loginEndPoint ?? throw new ArgumentNullException(nameof(loginEndPoint));
            this.registerEndPoint = registerEndPoint ?? throw new ArgumentNullException(nameof(registerEndPoint));
        }

        /// <summary>
        /// Post method for authorization, created LoginDTO before using
        /// </summary>
        /// <param name="loginDTO"></param>
        /// <returns></returns>
        public async UniTask<AuthResponseDTO> LoginAsync(LoginDTO loginDTO)
        {
            AuthResponseDTO authResponse;
            try
            {
                authResponse = await _apiService.PostAsync<LoginDTO, AuthResponseDTO>(loginEndPoint, loginDTO, new()
                {
                    Headers = new()
                    {
                        { "Content-Type", "application/json" }
                    }
                });
                
            }
            catch (Exception ex)
            {
                authResponse = new AuthResponseDTO(false, ex.Message);
            }
            return authResponse;
        }

        /// <summary>
        /// Post method for registration, created RegisterDTO before using
        /// </summary>
        /// <param name="registerDTO"></param>
        /// <returns></returns>
        public async UniTask<AuthResponseDTO> RegisterAsync(RegisterDTO registerDTO)
        {
            AuthResponseDTO authResponse;
            try
            {
                authResponse = await _apiService.PostAsync<RegisterDTO, AuthResponseDTO>(registerEndPoint, registerDTO, new()
                {
                    Headers = new()
                {
                    { "Content-Type", "application/json" }
                }
                });
            }
            catch (Exception ex)
            {
                authResponse = new AuthResponseDTO(false, ex.Message);
            }
            return authResponse;
        }
    }
}