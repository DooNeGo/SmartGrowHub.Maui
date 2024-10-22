using SmartGrowHub.Domain.Extensions;

namespace SmartGrowHub.Maui;

public sealed partial class AppShell
{
    public AppShell() => InitializeComponent();

    public IO<Unit> SetMainAsRoot(bool animate = true, CancellationToken cancellationToken = default) =>
        MainThread.InvokeOnMainThreadAsync(() =>
            GoToAsync("//MainTabBar", animate).WaitAsync(cancellationToken)).ToIO();

    public IO<Unit> SetLogInAsRoot(bool animate = true, CancellationToken cancellationToken = default) =>
        MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await GoToAsync("//StartPage", animate).WaitAsync(cancellationToken);
            return;
        }).ToIO();
}
