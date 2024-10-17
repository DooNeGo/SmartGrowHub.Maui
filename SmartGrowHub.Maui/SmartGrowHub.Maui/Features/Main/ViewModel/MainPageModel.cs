using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mediator;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Maui.Application.Interfaces;
using SmartGrowHub.Maui.Application.Messages.Commands;

namespace SmartGrowHub.Maui.Features.Main.ViewModel;

public sealed partial class MainPageModel(
    IDialogService dialogService,
    IUserSessionProvider sessionProvider,
    IMediator mediator)
    : ObservableObject
{
    [RelayCommand]
    public async Task<Unit> LogoutAsync(CancellationToken cancellationToken)
    {
        await dialogService.ShowLoadingAsync().RunAsync().ConfigureAwait(false);
        await Task.Run(() => LogOut(cancellationToken).RunUnsafeAsync())
            .ConfigureAwait(false);

        return dialogService.Pop().Run();
    }

    private Eff<Unit> LogOut(CancellationToken cancellationToken) =>
        from sessionId in sessionProvider.GetUserSessionId(cancellationToken)
        from _ in mediator.Send(LogOutCommand.Default, cancellationToken).ToEff()
        select unit;

    [RelayCommand]
    public Task<Unit> IsTokenExpired(CancellationToken cancellationToken) =>
        Task.Run(() => sessionProvider
            .GetAccessTokenIfNotExpired(cancellationToken)
            .Bind(token => dialogService.DisplayAlert("Token", token, "Ok"))
            .IfFailEff(error => dialogService.DisplayAlert("Token", error.IsEmpty ? "Expired" : error.Message, "Ok"))
            .RunUnsafeAsync()
            .AsTask(),
            cancellationToken);
}