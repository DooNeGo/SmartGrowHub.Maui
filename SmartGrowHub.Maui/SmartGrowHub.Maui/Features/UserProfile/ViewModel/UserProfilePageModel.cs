using CommunityToolkit.Mvvm.ComponentModel;

namespace SmartGrowHub.Maui.Features.UserProfile.ViewModel;

public sealed partial class UserProfilePageModel : ObservableObject
{
    /*private readonly IUserService _userService;
    private readonly IDialogService _dialogService;
    private readonly IUserSessionService _sessionService;
    private readonly ILogOutService _logOutService;

    [ObservableProperty] private User? _user;
    [ObservableProperty] private bool _isRefreshing;

    public UserProfilePageModel(
        IUserService userService,
        IDialogService dialogService,
        IUserSessionService sessionService,
        ILogOutService logOutService,
        IMessenger messenger)
    {
        _userService = userService;
        _dialogService = dialogService;
        _sessionService = sessionService;
        _logOutService = logOutService;
        
        RefreshWithLoadingAsync(CancellationToken.None).SafeFireAndForget();
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
        from userId in _sessionService.GetUserId(cancellationToken)
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
    }*/
}
