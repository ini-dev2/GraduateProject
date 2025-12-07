using System;
using Zenject;

namespace CoreBox
{
    internal sealed class LoginPresenter : IDisposable
    {
        private readonly ILoginUseCase _loginUseCase;
        private readonly ILoginView _loginView;
        private readonly NavigationService _navigationService;
        private readonly ScenarioPresenter _scenarioPresenter;

        [Inject]
        public LoginPresenter(ILoginUseCase loginUseCase, ILoginView loginView, NavigationService navigationService, ScenarioPresenter scenarioPresenter)
        {
            _loginUseCase = loginUseCase ?? throw new ArgumentNullException(nameof(loginUseCase));
            _loginView = loginView ?? throw new ArgumentNullException(nameof(loginView));

            _loginView.OnLoginPressed += HandleLoginPressed;
            _navigationService = navigationService;
            _scenarioPresenter = scenarioPresenter;
        }

        private async void HandleLoginPressed(string email, string password)
        {
            var result = await _loginUseCase.ExecuteAsync(email, password);

            if (result.IsSuccess)
            {
                _scenarioPresenter.GetScenarios();
                _loginView.OnSuccess($"Welcome, {result.UserName}!");
                _navigationService.SetScenariosWindow();
            }
            else
                _loginView.OnError(result.StatusMessage);
        }

        public void Dispose()
        {
            _loginView.OnLoginPressed -= HandleLoginPressed;
        }
    }
}