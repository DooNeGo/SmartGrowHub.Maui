﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGrowHub.Application.Register;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Common.Password;
using SmartGrowHub.Maui.Application.Interfaces;
using SmartGrowHub.Maui.Features.LogIn.ViewModel;

namespace SmartGrowHub.Maui.Features.Register.ViewModel;

public sealed partial class RegisterPageModel(
    INavigationService navigationService,
    IDialogService dialogService,
    IRegisterService registerService)
    : ObservableObject
{
    [ObservableProperty] private string _userNameRaw = string.Empty;
    [ObservableProperty] private string _passwordRaw = string.Empty;
    [ObservableProperty] private string _emailRaw = string.Empty;
    [ObservableProperty] private string _displayNameRaw = string.Empty;

    [ObservableProperty] private string _userNameError = string.Empty;
    [ObservableProperty] private string _passwordError = string.Empty;
    [ObservableProperty] private string _emailError = string.Empty;
    [ObservableProperty] private string _displayNameError = string.Empty;

    [RelayCommand]
    private Task<Unit> GoToLogInPageAsync() => navigationService
        .GoToAsync(nameof(LogInPageModel))
        .RunAsync().AsTask();

    [RelayCommand]
    private async Task RegisterAsync(CancellationToken cancellationToken)
    {
        await dialogService.ShowLoadingAsync().RunAsync().ConfigureAwait(false);
        await Register(cancellationToken).RunAsync().ConfigureAwait(false);
        await dialogService.Pop().RunAsync().ConfigureAwait(false);
    }

    private Eff<Unit> Register(CancellationToken cancellationToken) =>
        from userName in CreateDomainType<UserName>(UserNameRaw, error => UserNameError = error.Message)
        from password in CreateDomainType<PlainTextPassword>(PasswordRaw, error => PasswordError = error.Message)
        from email in CreateDomainType<EmailAddress>(EmailRaw, error => EmailError = error.Message)
        from displayName in CreateDomainType<NonEmptyString>(DisplayNameRaw, error => DisplayNameError = error.Message)
        let request = new RegisterRequest(userName, password, email, displayName)
        from _ in registerService.Register(request, cancellationToken)
        select unit;

    private static Eff<T> CreateDomainType<T>(string repr, Action<Error> onError)
        where T : DomainType<T, string> =>
        T.From(repr).BindFail(error =>
        {
            onError(error);
            return error;
        });
}