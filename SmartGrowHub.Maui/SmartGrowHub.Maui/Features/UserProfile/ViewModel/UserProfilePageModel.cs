using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Maui.Application.Interfaces;
using SmartGrowHub.Maui.Application.Messages;

namespace SmartGrowHub.Maui.Features.UserProfile.ViewModel;

public sealed partial class UserProfilePageModel : ObservableObject
{
    private readonly IUserService _userService;
    private readonly IDialogService _dialogService;
    private readonly IUserSessionProvider _sessionProvider;
    private readonly ILogOutService _logOutService;

    [ObservableProperty] private User? _user;
    [ObservableProperty] private bool _isRefreshing;

    public UserProfilePageModel(
        IUserService userService,
        IDialogService dialogService,
        IUserSessionProvider sessionProvider,
        ILogOutService logOutService,
        IMessenger messenger)
    {
        _userService = userService;
        _dialogService = dialogService;
        _sessionProvider = sessionProvider;
        _logOutService = logOutService;

        RegisterMessages(messenger);
        RefreshWithLoadingAsync(CancellationToken.None).SafeFireAndForget();
    }

    private Unit RegisterMessages(IMessenger messenger)
    {
        messenger.Register<LoggedOutMessage>(this, (_, _) =>
        {
            User = null;
            RefreshCommand.Cancel();
            IsRefreshing = false;
        });

        messenger.Register<LoggedInMessage>(this, (_, _) =>
        {
            IsRefreshing = true;
        });

        return unit;
    }

    private async Task RefreshWithLoadingAsync(CancellationToken cancellationToken)
    {
        await _dialogService.ShowLoadingAsync().RunAsync().ConfigureAwait(false);
        await RefreshAsync(cancellationToken).ConfigureAwait(false);
        await _dialogService.Pop().RunAsync().ConfigureAwait(false);
    }

    [RelayCommand]
    private async Task RefreshAsync(CancellationToken cancellationToken)
    {
        Fin<User> userFin = await GetUser(cancellationToken)
            .Bind(SetUser)
            .RunAsync()
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
    private async Task LogoutAsync(CancellationToken cancellationToken)
    {
        await _dialogService.ShowLoadingAsync().RunAsync().ConfigureAwait(false);
        await _logOutService.LogOut(cancellationToken).RunAsync().ConfigureAwait(false);
        await _dialogService.Pop().RunAsync().ConfigureAwait(false);
    }
}
