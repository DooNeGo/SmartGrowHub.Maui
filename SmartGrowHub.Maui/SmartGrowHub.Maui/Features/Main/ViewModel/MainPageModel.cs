using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGrowHub.Maui.Services;
using SmartGrowHub.Maui.Services.Abstractions;

namespace SmartGrowHub.Maui.Features.Main.ViewModel;

public sealed partial class MainPageModel(
    IAuthService authService,
    IDialogService dialogService)
    : ObservableObject
{
    [RelayCommand]
    public void Logout()
    {
        _ = authService.Logout().IfFailThrow();
    }
}