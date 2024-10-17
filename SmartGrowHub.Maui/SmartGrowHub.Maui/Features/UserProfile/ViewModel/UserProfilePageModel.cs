using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGrowHub.Domain.Model;
using SmartGrowHub.Maui.Application.Interfaces;

namespace SmartGrowHub.Maui.Features.UserProfile.ViewModel;

public sealed partial class UserProfilePageModel : ObservableObject
{
    private readonly IUserService _userService;
    private readonly IDialogService _dialogService;
    private readonly IUserSessionProvider _sessionProvider;

    [ObservableProperty] private User? _user;
    [ObservableProperty] private bool _isRefreshing;

    public UserProfilePageModel(
        IUserService userService,
        IDialogService dialogService,
        IUserSessionProvider sessionProvider)
    {
        _userService = userService;
        _dialogService = dialogService;
        _sessionProvider = sessionProvider;

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
                .RunAsync());

        IsRefreshing = false;
    }

    private Eff<User> GetUser(CancellationToken cancellationToken) =>
        from userId in _sessionProvider.GetUserId(cancellationToken)
        from user in _userService.GetUser(userId, cancellationToken)
        select user;

    private IO<User> SetUser(User user) =>
        lift(() => User = user);
}
