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
    public async Task LogoutAsync(CancellationToken cancellationToken)
    {
        await using IAsyncDisposable loading = await dialogService.Loading();
        authService.Logout().Try();
    }
}