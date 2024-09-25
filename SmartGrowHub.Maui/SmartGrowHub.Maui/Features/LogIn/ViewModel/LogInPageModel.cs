using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGrowHub.Domain.Features.LogIn;
using SmartGrowHub.Maui.Features.Register.ViewModel;
using SmartGrowHub.Maui.Services;
using SmartGrowHub.Maui.Services.Abstractions;
using SmartGrowHub.Shared.Auth.Dto.LogIn;
using SmartGrowHub.Shared.Auth.Extensions;
using Resources = SmartGrowHub.Maui.Localization.Resources;

namespace SmartGrowHub.Maui.Features.LogIn.ViewModel;

public sealed partial class LogInPageModel(
    AppShell shell,
    IServiceProvider serviceProvider,
    IDialogService dialogService)
    : ObservableObject
{
    [ObservableProperty] private string _userNameRaw = string.Empty;
    [ObservableProperty] private string _passwordRaw = string.Empty;
    [ObservableProperty] private bool _remember;

    [RelayCommand]
    private Task GoToRegisterPageAsync() => shell.GoToAsync(nameof(RegisterPageModel));

    [RelayCommand]
    private Task GoToMainPageAsync() =>
        Application.Current!.Dispatcher.DispatchAsync(() =>
            shell.GoToAsync("///MainTabBar"));

    [RelayCommand]
    private void LogIn()
    {
        Fin<LogInRequest> requestFin = new LogInRequestDto(UserNameRaw, PasswordRaw).TryToDomain();

        IDisposable loading = dialogService.Loading();
        AsyncServiceScope scope = serviceProvider.CreateAsyncScope();
        IAuthService authService = scope.ServiceProvider.GetRequiredService<IAuthService>();
        CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(15));

        requestFin
            .Match(
                Succ: request => authService
                    .LogInAsync(request, Remember, tokenSource.Token)
                    .Map(_ =>
                        unit),
                Fail: error => Pure(unit))
            .Bind(_ => liftEff(() =>
            {
                loading.Dispose();
                scope.DisposeAsync().SafeFireAndForget();
                tokenSource.Dispose();
            }))
            .RunUnsafeAsync()
            .SafeFireAndForget();
    }

    private Unit DisplayAlert(string message) =>
        dialogService.DisplayAlert(Resources.Authorization, message, Resources.Ok);
}
