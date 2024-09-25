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
    INavigationService navigationService,
    IServiceProvider serviceProvider,
    IDialogService dialogService)
    : ObservableObject
{
    [ObservableProperty] private string _userNameRaw = string.Empty;
    [ObservableProperty] private string _passwordRaw = string.Empty;
    [ObservableProperty] private bool _remember;

    [RelayCommand]
    private Task<Unit> GoToRegisterPageAsync() => navigationService
        .GoToAsync(nameof(RegisterPageModel))
        .RunUnsafeAsync()
        .AsTask();

    [RelayCommand]
    private async Task<Unit> LogInAsync()
    {
        Fin<LogInRequest> requestFin = new LogInRequestDto(UserNameRaw, PasswordRaw).TryToDomain();

        using IDisposable loading = dialogService.Loading().RunUnsafe();
        var authService = serviceProvider.GetRequiredService<IAuthService>();
     
        using CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(15));
        
        return await Task.Run(() => requestFin.ToEff()
            .Bind(request => authService
                .LogInAsync(request, Remember, tokenSource.Token)
                .Map(_ => unit))
            .RunAsync()
            .Map(fin => fin.IfFail(error => DisplayAlert(error.Message))),
            tokenSource.Token);
    }

    private Unit DisplayAlert(string message) =>
        dialogService.DisplayAlert(Resources.Authorization, message, Resources.Ok);
}
