using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using SmartGrowHub.Maui.Application.Interfaces;

namespace SmartGrowHub.Maui.Features.AppStart.ViewModel;

public sealed partial class AppStartPageModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly IUserSessionService _sessionService;

    public AppStartPageModel(
        INavigationService navigationService,
        IUserSessionService sessionService)
    {
        _navigationService = navigationService;
        _sessionService = sessionService;

        InitializeAsync(CancellationToken.None).SafeFireAndForget();
    }

    private Task<Fin<Unit>> InitializeAsync(CancellationToken cancellationToken) =>
        _sessionService
            .GetUserSession(cancellationToken)
            .Bind(_ => _navigationService.SetMainPageAsRoot(true, cancellationToken))
            .IfFailEff(_ => _navigationService.SetLogInAsRoot(true, cancellationToken))
            .RunAsync();
}
