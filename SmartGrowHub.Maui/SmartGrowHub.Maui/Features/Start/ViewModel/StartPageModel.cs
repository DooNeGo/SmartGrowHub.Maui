using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGrowHub.Maui.Features.LogIn.ViewModel;
using SmartGrowHub.Maui.Features.Register.ViewModel;
using SmartGrowHub.Maui.Services;

namespace SmartGrowHub.Maui.Features.Start.ViewModel;

public sealed partial class StartPageModel(INavigationService navigationService)
    : ObservableObject
{
    [RelayCommand]
    private Task<Unit> GoToLogInPageAsync() => navigationService
        .GoToAsync(nameof(LogInPageModel))
        .RunUnsafeAsync()
        .AsTask();

    [RelayCommand]
    private Task<Unit> GoToRegisterPageAsync() => navigationService
        .GoToAsync(nameof(RegisterPageModel))
        .RunUnsafeAsync()
        .AsTask();
}