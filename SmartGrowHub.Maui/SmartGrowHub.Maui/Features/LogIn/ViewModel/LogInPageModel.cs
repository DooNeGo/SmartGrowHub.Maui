using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Requests;
using SmartGrowHub.Maui.Features.Register.ViewModel;
using SmartGrowHub.Maui.Localization;
using SmartGrowHub.Maui.Services;
using SmartGrowHub.Maui.Services.Abstractions;

namespace SmartGrowHub.Maui.Features.LogIn.ViewModel;

public sealed partial class LogInPageModel(
    AppShell shell,
    IAuthService authService,
    IDialogService dialogService)
    : ObservableValidator
{
    [ObservableProperty] private string _userNameRaw = string.Empty;
    [ObservableProperty] private string _passwordRaw = string.Empty;
    [ObservableProperty] private bool _remember;

    [RelayCommand]
    private Task GoToRegisterPageAsync() => shell.GoToAsync(nameof(RegisterPageModel));

    [RelayCommand]
    private Task GoToMainPageAsync(CancellationToken cancellationToken) =>
        Application.Current.Dispatcher.DispatchAsync(() => shell
            .GoToAsync("///MainTabBar")
            .WaitAsync(cancellationToken));

    [RelayCommand]
    private async Task<Unit> LogInAsync(CancellationToken cancellationToken)
    {
        await using IAsyncDisposable loading = await dialogService.Loading();

        Fin<LogInRequest> requestFin =
            from userName in UserName.Create(UserNameRaw)
            from password in Password.Create(PasswordRaw)
            select new LogInRequest(userName, password);

        return await requestFin.ToEitherAsync().MatchAsync(
            RightAsync: request => authService
                .LogInAsync(request, Remember, cancellationToken)
                .MatchAsync(
                    SomeAsync: _ => GoToMainPageAsync(cancellationToken).ToUnit(),
                    NoneAsync: () => DisplayAlertAsync(Resources.UserNotFound, cancellationToken).ToUnit(),
                    FailAsync: exception => DisplayAlertAsync(exception.Message, cancellationToken).ToUnit()),
            LeftAsync: error => DisplayAlertAsync(error.Message, cancellationToken));
    }

    private Task DisplayAlertAsync(string message, CancellationToken cancellationToken) =>
        dialogService.DisplayAlertAsync(Resources.Authorization, message, Resources.Ok, cancellationToken);
}
