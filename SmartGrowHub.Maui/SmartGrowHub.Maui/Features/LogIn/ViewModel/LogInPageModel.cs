using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGrowHub.Maui.Features.Register.ViewModel;

namespace SmartGrowHub.Maui.Features.LogIn.ViewModel;

public sealed partial class LogInPageModel(Shell shell) : ObservableObject
{
    [ObservableProperty] private string _userName = string.Empty;
    [ObservableProperty] private string _password = string.Empty;

    [RelayCommand]
    private Task GoToRegisterPageAsync() => shell.GoToAsync(nameof(RegisterPageModel));
}
