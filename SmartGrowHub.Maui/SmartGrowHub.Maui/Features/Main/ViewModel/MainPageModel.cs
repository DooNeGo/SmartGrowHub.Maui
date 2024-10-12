using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGrowHub.Maui.Services;
using SmartGrowHub.Maui.Services.Abstractions;

namespace SmartGrowHub.Maui.Features.Main.ViewModel;

public sealed partial class MainPageModel(
    IDialogService dialogService,
    IAuthService authService,
    IUserSessionProvider sessionProvider)
    : ObservableObject
{
    [RelayCommand]
    public Task<Unit> LogoutAsync(CancellationToken cancellationToken) =>
        Task.Run(() => (
            from _1 in dialogService.ShowLoading()
            from _2 in authService.LogOutAsync(cancellationToken)
            select unit)
            .RunUnsafeAsync()
            .Bind(_ => dialogService.PopAsync().RunAsync())
            .AsTask(), cancellationToken);

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