namespace SmartGrowHub.Maui;

public sealed partial class AppShell
{
    public AppShell() => InitializeComponent();

    public Task<Unit> SetMainAsRootAsync(bool animate = true, CancellationToken cancellationToken = default) =>
        GoToMainTabBarAsync(animate, cancellationToken);

    public Task<Unit> SetLogInAsRootAsync(bool animate = true, CancellationToken cancellationToken = default) =>
        GoToLogInAsync(animate, cancellationToken);

    public Task<Unit> SetMainAsRoot(bool animate = true, CancellationToken cancellationToken = default) =>
        GoToMainTabBarAsync(animate, cancellationToken);

    public Task<Unit> SetLogInAsRoot(bool animate = true, CancellationToken cancellationToken = default) =>
        GoToLogInAsync(animate, cancellationToken);

    private Task<Unit> GoToLogInAsync(bool animate = true, CancellationToken cancellationToken = default) =>
        GoToAsync("//StartPage", animate).WaitAsync(cancellationToken).ToUnit();

    private Task<Unit> GoToMainTabBarAsync(bool animate = true, CancellationToken cancellationToken = default) =>
        GoToAsync("//MainTabBar", animate).WaitAsync(cancellationToken).ToUnit();
}
