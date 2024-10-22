using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mediator;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Maui.Application.Commands;
using SmartGrowHub.Maui.Application.Interfaces;
using SmartGrowHub.Maui.Application.Messages;

namespace SmartGrowHub.Maui.Features.UserProfile.ViewModel;

public sealed partial class UserProfilePageModel : ObservableObject
{
    private readonly IUserService _userService;
    private readonly IDialogService _dialogService;
    private readonly IUserSessionProvider _sessionProvider;
    private readonly IMediator _mediator;

    [ObservableProperty] private User? _user;
    [ObservableProperty] private bool _isRefreshing;

    public UserProfilePageModel(
        IUserService userService,
        IDialogService dialogService,
        IUserSessionProvider sessionProvider,
        IMediator mediator,
        IMessenger messenger)
    {
        _userService = userService;
        _dialogService = dialogService;
        _sessionProvider = sessionProvider;
        _mediator = mediator;

        messenger.Register<LoggedOutMessage>(this, (_, _) =>
        {
            User = null;
            RefreshCommand.Cancel();
            IsRefreshing = false;
        });

        RefreshWithLoadingAsync(CancellationToken.None).SafeFireAndForget();
    }

    private async Task RefreshWithLoadingAsync(CancellationToken cancellationToken)
    {
        await _dialogService.ShowLoadingAsync().RunAsync().ConfigureAwait(false);
        await RefreshAsync(cancellationToken).ConfigureAwait(false);
        _dialogService.Pop().Run();
    }

    [RelayCommand]
    private async Task RefreshAsync(CancellationToken cancellationToken)
    {
        Fin<User> userFin = await Task.Run(() =>
            GetUser(cancellationToken)
                .Bind(user => SetUser(user))
                .RunAsync())
            .ConfigureAwait(false);

        IsRefreshing = false;
    }

    private Eff<User> GetUser(CancellationToken cancellationToken) =>
        from userId in _sessionProvider.GetUserId(cancellationToken)
        from user in _userService.GetUser(userId, cancellationToken)
        select user;

    private IO<User> SetUser(User user) =>
        lift(() => User = user);

    [RelayCommand]
    private async Task<Unit> LogoutAsync(CancellationToken cancellationToken)
    {
        await _dialogService.ShowLoadingAsync().RunAsync().ConfigureAwait(false);
        await LogOut(cancellationToken).RunAsync().ConfigureAwait(false);

        return await _dialogService.Pop().RunAsync().ConfigureAwait(false);
    }

    private Eff<Unit> LogOut(CancellationToken cancellationToken) =>
        _mediator.Send(LogOutCommand.Default, cancellationToken).ToEff();
}
