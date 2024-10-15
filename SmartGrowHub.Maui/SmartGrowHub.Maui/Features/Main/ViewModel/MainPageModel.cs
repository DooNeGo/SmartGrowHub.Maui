using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGrowHub.Application.LogOut;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Maui.Application.Interfaces;

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
            from sessionId in sessionProvider.GetUserSessionId(cancellationToken)
            from _3 in authService.LogOut(new LogOutRequest(sessionId), cancellationToken)
            select unit)
            .RunUnsafeAsync()
            .Bind(_ => dialogService.PopAsync().RunAsync())
            .AsTask(), cancellationToken);

    [RelayCommand]
    public Task<Unit> IsTokenExpired(CancellationToken cancellationToken) =>
        Task.Run(() => sessionProvider
            .GetAccessTokenIfNotExpired(cancellationToken)
            .Bind(token => dialogService.DisplayAlert("Token", token, "Ok"))
            .IfFailEff(error => dialogService.DisplayAlert("Token", error.IsEmpty ? "Expired" : error.Message, "Ok"))
            .RunUnsafeAsync()
            .AsTask(),
            cancellationToken);
}