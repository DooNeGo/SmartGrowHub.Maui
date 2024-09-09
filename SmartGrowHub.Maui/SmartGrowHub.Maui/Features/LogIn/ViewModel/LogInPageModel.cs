﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Requests;
using SmartGrowHub.Maui.Features.Register.ViewModel;
using SmartGrowHub.Maui.Services.Abstractions;

namespace SmartGrowHub.Maui.Features.LogIn.ViewModel;

public sealed partial class LogInPageModel(Shell shell, IAuthService authService)
    : ObservableValidator
{
    [ObservableProperty] private string _userNameRaw = string.Empty;
    [ObservableProperty] private string _passwordRaw = string.Empty;

    [RelayCommand]
    private Task GoToRegisterPageAsync() => shell.GoToAsync(nameof(RegisterPageModel));

    [RelayCommand]
    private Task GoToMainPageAsync(CancellationToken cancellationToken) =>
        shell.GoToAsync("///MainTabBar").WaitAsync(cancellationToken);

    [RelayCommand]
    private Task<Unit> LogInAsync(CancellationToken cancellationToken)
    {
        Fin<LogInRequest> request =
            from userName in UserName.Create(UserNameRaw)
            from password in Password.Create(PasswordRaw)
            select new LogInRequest(userName, password);

        return request.ToEitherAsync().MatchAsync(
            RightAsync: request => authService
                .LogInAsync(request, cancellationToken)
                .MatchAsync(
                    SomeAsync: response => GoToMainPageAsync(cancellationToken).ToUnit(),
                    NoneAsync: () => DisplayAlert("User not found", cancellationToken).ToUnit(),
                    FailAsync: exception => DisplayAlert(exception.Message, cancellationToken).ToUnit()),
            LeftAsync: error => DisplayAlert(error.Message, cancellationToken));
    }

    private static Task DisplayAlert(string message, CancellationToken cancellationToken) =>
        Application.Current.MainPage.DisplayAlert("Log in", message, "Ok").WaitAsync(cancellationToken);
}
