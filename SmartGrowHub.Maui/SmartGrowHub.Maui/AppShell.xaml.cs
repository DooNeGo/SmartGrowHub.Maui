using SmartGrowHub.Domain.Extensions;

namespace SmartGrowHub.Maui;

public sealed partial class AppShell
{
    public AppShell() => InitializeComponent();

    public IO<Unit> SetMainAsRootAsync(bool animate = true, CancellationToken cancellationToken = default) =>
        MainThread.InvokeOnMainThreadAsync(() =>
            GoToMainTabBarAsync(animate, cancellationToken)).ToIO();

    public IO<Unit> SetLogInAsRootAsync(bool animate = true, CancellationToken cancellationToken = default) =>
        MainThread.InvokeOnMainThreadAsync(() =>
            GoToLogInAsync(animate, cancellationToken)).ToIO();

    public IO<Unit> SetMainAsRoot(bool animate = true, CancellationToken cancellationToken = default) =>
        lift(() => MainThread.BeginInvokeOnMainThread(() =>
            GoToMainTabBarAsync(animate, cancellationToken)));

    public IO<Unit> SetLogInAsRoot(bool animate = true, CancellationToken cancellationToken = default) =>
        lift(() => MainThread.BeginInvokeOnMainThread(() =>
            GoToLogInAsync(animate, cancellationToken)));

    private Task GoToLogInAsync(bool animate = true, CancellationToken cancellationToken = default) =>
        GoToAsync("//StartPage", animate).WaitAsync(cancellationToken);

    private Task GoToMainTabBarAsync(bool animate = true, CancellationToken cancellationToken = default) =>
        GoToAsync("//MainTabBar", animate).WaitAsync(cancellationToken);
}
