using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGrowHub.Maui.Services.App;

namespace SmartGrowHub.Maui.Features.Login.ViewModel;

public sealed partial class StartPageModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    
    public StartPageModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    [RelayCommand]
    private Task<Unit> LogInByEmailAsync() =>
        _navigationService
            .Navigate(Routes.RequestOtpToEmailPage)
            .RunAsync().AsTask();
}