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
    private Task<Unit> GoToRegisterPageAsync() => navigationService
        .GoToAsync(nameof(RegisterPageModel))
        .RunUnsafeAsync()
        .AsTask();

    [RelayCommand]
    private Task<Unit> LogInAsync()
    {
        CancellationTokenSource tokenSource = new(TimeSpan.FromSeconds(5));

        return Task.Run(() =>
            (from request in LogInRequest.Create(UserNameRaw, PasswordRaw).ToEff()
             from _1 in dialogService.ShowLoading()
             from _2 in authService.LogInAsync(request, Remember, tokenSource.Token)
             select unit)
             .IfFailEff(error => DisplayAlert(error.Message))
             .RunUnsafeAsync()
             .Bind(_ => dialogService.HideLoading().RunAsync())
             .AsTask()
             .Map(_ =>
             {
                 tokenSource.Dispose();
                 return unit;
             }));
    }

    private IO<Unit> DisplayAlert(string message) =>
        dialogService.DisplayAlert(Resources.Authorization, message, Resources.Ok);
}
