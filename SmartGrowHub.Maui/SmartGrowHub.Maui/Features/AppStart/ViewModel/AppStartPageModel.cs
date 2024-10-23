using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using SmartGrowHub.Maui.Application.Interfaces;

namespace SmartGrowHub.Maui.Features.AppStart.ViewModel;

public sealed partial class AppStartPageModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly IUserSessionProvider _sessionProvider;

    public AppStartPageModel(
        INavigationService navigationService,
        IUserSessionProvider sessionProvider)
    {
        _navigationService = navigationService;
        _sessionProvider = sessionProvider;

        InitializeAsync(CancellationToken.None).SafeFireAndForget();
    }

    private Task<Fin<Unit>> InitializeAsync(CancellationToken cancellationToken) =>
        _sessionProvider
            .GetUserSession(cancellationToken)
            .Bind(_ => _navigationService.SetMainPageAsRoot(true, cancellationToken))
            .IfFailEff(_ => _navigationService.SetLogInAsRoot(true, cancellationToken))
            .RunAsync();
}
