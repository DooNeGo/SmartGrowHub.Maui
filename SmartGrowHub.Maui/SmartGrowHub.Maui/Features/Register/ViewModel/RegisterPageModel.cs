using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGrowHub.Maui.Features.LogIn.ViewModel;

namespace SmartGrowHub.Maui.Features.Register.ViewModel;

public sealed partial class RegisterPageModel(Shell shell) : ObservableObject
{
    [RelayCommand]
    private Task GoToLogInPageAsync() => shell.GoToAsync(nameof(LogInPageModel));
}