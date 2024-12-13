using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using SmartGrowHub.Maui.Services.App;
using SmartGrowHub.Maui.Services.Infrastructure;

namespace SmartGrowHub.Maui.Features.Loading.ViewModel;

public sealed partial class LoadingPageModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly ITokensStorage _tokensStorage;

    public LoadingPageModel(
        INavigationService navigationService,
        ITokensStorage tokensStorage)
    {
        _navigationService = navigationService;
        _tokensStorage = tokensStorage;

        InitializeAsync(CancellationToken.None).SafeFireAndForget();
    }

    private Task InitializeAsync(CancellationToken cancellationToken) =>
        _tokensStorage
            .GetAccessToken(cancellationToken)
            .Bind(_ => _navigationService.SetMainPageAsRoot(true, cancellationToken))
            .IfFailEff(_ => _navigationService.SetLogInAsRoot(true, cancellationToken))
            .RunAsync();
}
