using System;
using Zenject;

namespace CoreBox
{
    internal sealed class RegisterPresenter : IDisposable
    {
        private readonly IRegisterUseCase _registerUseCase;
        private readonly IRegisterView _registerView;
        private readonly NavigationService _navigationService;

        [Inject]
        public RegisterPresenter(IRegisterUseCase registerUseCase, IRegisterView registerView, NavigationService navigationService)
        {
            _registerUseCase = registerUseCase ?? throw new ArgumentNullException(nameof(registerUseCase));
            _registerView = registerView ?? throw new ArgumentNullException(nameof(registerView));

            _registerView.OnRegisterPressed += HandleRegisterPressed;
            _navigationService = navigationService;
        }

        private async void HandleRegisterPressed(string username, string email, string password)
        {
            var result = await _registerUseCase.ExecuteAsync(username, email, password);

            if (result.IsSuccess)
            {
                _registerView.OnSuccess($"Register, {result.UserName}!");
                _navigationService.SetAuthorizationWindow();
            }
                
            else
                _registerView.OnError(result.StatusMessage);
        }

        public void Dispose()
        {
            _registerView.OnRegisterPressed -= HandleRegisterPressed;
        }
    }
}