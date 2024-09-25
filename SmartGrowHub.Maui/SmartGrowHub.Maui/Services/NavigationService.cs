using SmartGrowHub.Maui.Services.Extensions;

namespace SmartGrowHub.Maui.Services;

public interface INavigationService
{
    Eff<Unit> GoBackAsync(CancellationToken cancellationToken = default);
    Eff<Unit> GoToAsync(string path, CancellationToken cancellationToken = default);
}

public sealed class NavigationService(AppShell shell) : INavigationService
{
    private readonly IDispatcher _dispatcher = shell.Dispatcher;

    public Eff<Unit> GoBackAsync(CancellationToken cancellationToken = default) =>
        liftEff(() => _dispatcher.InvokeOnUiThreadIfNeeded(
            () => shell.Navigation.PopAsync()
                .WaitAsync(cancellationToken)
                .ToUnit()));

    public Eff<Unit> GoToAsync(string path, CancellationToken cancellationToken = default) =>
        liftEff(() => _dispatcher.InvokeOnUiThreadIfNeeded(
            () => shell.GoToAsync(path)
                .WaitAsync(cancellationToken)
                .ToUnit()));
}