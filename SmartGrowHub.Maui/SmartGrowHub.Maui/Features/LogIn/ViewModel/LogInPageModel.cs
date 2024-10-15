using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGrowHub.Application.LogIn;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Maui.Application.Interfaces;
using SmartGrowHub.Maui.Features.Register.ViewModel;
using SmartGrowHub.Maui.Services;
using Resources = SmartGrowHub.Maui.Localization.Resources;

namespace SmartGrowHub.Maui.Features.LogIn.ViewModel;

public sealed partial class LogInPageModel(
    INavigationService navigationService,
    IAuthService authService,
    IDialogService dialogService)
    : ObservableObject
{
    [ObservableProperty] private string _userNameRaw = string.Empty;
    [ObservableProperty] private string _passwordRaw = string.Empty;
    [ObservableProperty] private bool _remember;

    [RelayCommand]
    private Task<Unit> GoToRegisterPageAsync(CancellationToken cancellationToken) =>
        navigationService
            .GoToAsync(nameof(RegisterPageModel), cancellationToken)
            .RunAsync().AsTask();

    [RelayCommand]
    private Task<Unit> LogInAsync(CancellationToken cancellationToken) =>
        Task.Run(() => (
            from request in LogInRequest.Create(UserNameRaw, PasswordRaw).ToEff()
            from _1 in dialogService.ShowLoadingAsync()
            from _2 in authService.LogIn(request, cancellationToken)
            select unit)
            .RunAsync()
            .Bind(fin => (dialogService.Pop() >>
                fin.Match(
                    Succ: _ => unitIO,
                    Fail: error => DisplayAlert(error.Message)))
                .RunAsync().AsTask()),
            cancellationToken);

    private IO<Unit> DisplayAlert(string message) =>
        dialogService.DisplayAlert(Resources.Authorization, message, Resources.Ok);
}
