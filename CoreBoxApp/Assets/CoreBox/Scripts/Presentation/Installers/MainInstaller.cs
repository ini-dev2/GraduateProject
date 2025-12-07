using CoreBox;
using RestBuilder;
using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [Header("=======================UI======================")]
    [SerializeField] private LoginView _loginView;
    [SerializeField] private RegisterView _registerView;
    [SerializeField] private NavigationService _navigationService;

    [Header("======================API======================")]
    [SerializeField] private string baseUrl;
    [SerializeField] private AuthServiceEndPoints authServiceEndPoints;
    public override void InstallBindings()
    {
        // === Infrastructure ===
        Container.Bind<IApiService>().To<ApiService>().AsSingle()
            .WithArguments(baseUrl)
            .NonLazy();

        Container.Bind<ITokenProvider>().To<TokenProvider>().AsSingle()
            .NonLazy();

        Container.Bind<IAuthService>().To<AuthService>().AsSingle()
            .WithArguments(authServiceEndPoints.LoginEndPoint, authServiceEndPoints.RegisterEndPoint)
            .NonLazy();

        // === Application / UseCase ===
        Container.Bind<ILoginUseCase>().To<LoginInteractor>().AsSingle()
            .NonLazy();
        Container.Bind<IRegisterUseCase>().To<RegisterInteractor>().AsSingle()
            .NonLazy();

        // === Presentation ===
        Container.Bind<ScenarioPresenter>().FromComponentInHierarchy().AsSingle()
            .NonLazy();

        Container.Bind<ILoginView>().FromInstance(_loginView).AsSingle()
            .NonLazy();
        Container.Bind<IRegisterView>().FromInstance(_registerView).AsSingle()
            .NonLazy();
        Container.Bind<NavigationService>().FromInstance(_navigationService).AsSingle()
            .NonLazy();

        Container.Bind<LoginPresenter>().AsSingle()
            .NonLazy();
        Container.Bind<RegisterPresenter>().AsSingle()
            .NonLazy();
    }
}