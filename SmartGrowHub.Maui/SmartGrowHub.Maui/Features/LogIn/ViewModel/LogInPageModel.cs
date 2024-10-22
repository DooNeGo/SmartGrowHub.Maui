using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mediator;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Common.Password;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Maui.Application.Commands;
using SmartGrowHub.Maui.Application.Interfaces;
using SmartGrowHub.Maui.Features.Register.ViewModel;

namespace SmartGrowHub.Maui.Features.LogIn.ViewModel;

public sealed partial class LogInPageModel(
    INavigationService navigationService,
    IMediator mediator,
    IDialogService dialogService)
    : ObservableObject
{
    [ObservableProperty] private string _userNameRaw = string.Empty;
    [ObservableProperty] private string _passwordRaw = string.Empty;
    [ObservableProperty] private bool _remember;

    [ObservableProperty] private string _userNameError = string.Empty;
    [ObservableProperty] private string _passwordError = string.Empty;

    [RelayCommand]
    private Task<Unit> GoToRegisterPageAsync(CancellationToken cancellationToken) =>
        navigationService
            .GoToAsync(nameof(RegisterPageModel), cancellationToken)
            .RunAsync().AsTask();

    [RelayCommand]
    private async Task LogInAsync(CancellationToken cancellationToken)
    {
        await dialogService.ShowLoadingAsync().RunAsync().ConfigureAwait(false);
        await LogIn(cancellationToken).RunAsync().ConfigureAwait(false);
        await dialogService.Pop().RunAsync().ConfigureAwait(false);
    }

    [RelayCommand]
    private void OnGoBack() => LogInCommand.Cancel();

    private Eff<Unit> LogIn(CancellationToken cancellationToken) =>
        from userName in CreateUserName(UserNameRaw).ToEff()
        from password in CreatePassword(PasswordRaw).ToEff()
        let command = new LogInCommand(userName, password, Remember)
        from _ in mediator
            .Send(command, cancellationToken).ToEff()
            .IfFailEff(error => DisplayError(error))
        select unit;

    private Fin<UserName> CreateUserName(string userNameRaw) =>
        CreateDomainType<UserName>(userNameRaw, error => UserNameError = error.Message);

    private Fin<PlainTextPassword> CreatePassword(string passwordRaw) =>
        CreateDomainType<PlainTextPassword>(passwordRaw, error => PasswordError = error.Message);

    private static Fin<T> CreateDomainType<T>(string repr, Action<Error> onError)
        where T : DomainType<T, string> =>
        T.From(repr).BindFail(error =>
        {
            onError(error);
            return error;
        });

    private IO<Unit> DisplayError(Error error) =>
        dialogService.DisplayAlert(
            Resources.Authorization,
            error.Message,
            Resources.Ok);
}
