using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGrowHub.Domain.Features.LogIn;
using SmartGrowHub.Maui.Features.Register.ViewModel;
using SmartGrowHub.Maui.Services;
using SmartGrowHub.Maui.Services.Abstractions;
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
            .RunUnsafeAsync()
            .AsTask();

    [RelayCommand]
    private Task<Unit> LogInAsync(CancellationToken cancellationToken) =>
        Task.Run(() => (
            from request in LogInRequest.Create(UserNameRaw, PasswordRaw).ToEff()
            from _1 in dialogService.ShowLoadingAsync()
            from _2 in authService.LogInAsync(request, Remember, cancellationToken)
            select unit)
            .RunAsync()
            .Bind(fin => dialogService
                .PopAsync()
                .RunAsync().AsTask()
                .Map(_ => fin.IfFail(error => DisplayAlert(error.Message).Run()))),
            cancellationToken);

    private IO<Unit> DisplayAlert(string message) =>
        dialogService.DisplayAlert(Resources.Authorization, message, Resources.Ok);
}
