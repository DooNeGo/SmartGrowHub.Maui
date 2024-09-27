using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGrowHub.Maui.Services;

namespace SmartGrowHub.Maui.Features.Main.ViewModel;

public sealed partial class MainPageModel(
    IDialogService dialogService,
    IUserSessionProvider sessionProvider)
    : ObservableObject
{
    [RelayCommand]
    public Task<Unit> LogoutAsync(CancellationToken cancellationToken) =>
        Task.Run(() => sessionProvider
            .Remove()
            .IfFailEff(error => dialogService.DisplayAlert("Logout error", error.Message, "Ok"))
            .RunUnsafeAsync()
            .AsTask(),
            cancellationToken);

    [RelayCommand]
    public Task<Unit> IsTokenExpired(CancellationToken cancellationToken) =>
        Task.Run(() => sessionProvider
            .GetAccessTokenIfNotExpiredAsync(cancellationToken)
            .Bind(option => option.Match(
                Some: token => dialogService.DisplayAlert("Token", token, "Ok"),
                None: () => dialogService.DisplayAlert("Token", "Expired", "Ok")))
            .IfFailEff(error => dialogService.DisplayAlert("Token", error.Message, "Ok"))
            .RunUnsafeAsync()
            .AsTask(),
            cancellationToken);
}