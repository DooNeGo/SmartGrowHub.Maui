using AsyncAwaitBestPractices;

namespace SmartGrowHub.Maui;

public sealed partial class AppShell
{
    public AppShell() => InitializeComponent();

    public void SetMainAsRoot(bool animate = true, CancellationToken cancellationToken = default) =>
        SetMainAsRootAsync(animate, cancellationToken).SafeFireAndForget();

    public void SetLoginAsRoot(bool animate = true, CancellationToken cancellationToken = default) =>
        SetLoginAsRootAsync(animate, cancellationToken).SafeFireAndForget();

    public Task<Unit> SetLoginAsRootAsync(bool animate = true, CancellationToken cancellationToken = default) =>
        GoToAsync("//Login", animate).WaitAsync(cancellationToken).ToUnit();

    public Task<Unit> SetMainAsRootAsync(bool animate = true, CancellationToken cancellationToken = default) =>
        GoToAsync("//Main", animate).WaitAsync(cancellationToken).ToUnit();
}
