using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGrowHub.Maui.Features.LogIn.ViewModel;
using SmartGrowHub.Maui.Features.Register.ViewModel;

namespace SmartGrowHub.Maui.Features.Start.ViewModel;

public sealed partial class StartPageModel(Shell shell) : ObservableObject
{
    [RelayCommand]
    private Task GoToLogInPageAsync() => shell.GoToAsync(nameof(LogInPageModel));

    [RelayCommand]
    private Task GoToRegisterPageAsync() => shell.GoToAsync(nameof(RegisterPageModel));
}