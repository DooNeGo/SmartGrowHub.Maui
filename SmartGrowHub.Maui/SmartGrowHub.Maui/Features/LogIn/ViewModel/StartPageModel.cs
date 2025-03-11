using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGrowHub.Maui.Services.App;

namespace SmartGrowHub.Maui.Features.LogIn.ViewModel;

public sealed partial class StartPageModel(INavigationService navigationService) : ObservableObject
{
    [RelayCommand]
    private Task<Unit> LogInByEmailAsync() =>
        navigationService.NavigateAsync(Routes.LoginByEmailPage).RunAsync().AsTask();
}