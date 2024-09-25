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
    public Task<Unit> Logout(CancellationToken cancellationToken) =>
        sessionProvider.Remove()
            .RunAsync()
            .WaitAsync(cancellationToken)
            .Map(fin => fin
                .IfFail(error => dialogService
                    .DisplayAlert("Logout error", error.Message, "Ok")));

    [RelayCommand]
    public Task<Unit> IsTokenExpired(CancellationToken cancellationToken) =>
        sessionProvider
            .GetAccessTokenIfNotExpiredAsync(cancellationToken)
            .Map(option => option.Match(
                Some: token => dialogService.DisplayAlert("Token", token, "Ok"),
                None: () => dialogService.DisplayAlert("Token", "Expired", "Ok")))
            .RunAsync()
            .Map(fin => fin
                .IfFail(error => dialogService
                    .DisplayAlert("Token1", error.Message, "Ok")));
}