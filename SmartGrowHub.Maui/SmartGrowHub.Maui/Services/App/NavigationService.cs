using SmartGrowHub.Maui.Services.Infrastructure;

namespace SmartGrowHub.Maui.Services.App;

public interface INavigationService
{
    IO<Unit> GoBackAsync(CancellationToken cancellationToken = default);
    IO<Unit> GoToAsync(string path, CancellationToken cancellationToken = default);
    IO<Unit> SetLogInAsRootAsync(bool animate = true, CancellationToken cancellationToken = default);
    IO<Unit> SetMainPageAsRootAsync(bool animate = true, CancellationToken cancellationToken = default);
    IO<Unit> SetLogInAsRoot(bool animate = true, CancellationToken cancellationToken = default);
    IO<Unit> SetMainPageAsRoot(bool animate = true, CancellationToken cancellationToken = default);
}


public sealed class NavigationService(AppShell shell, IMainThreadService mainThread) : INavigationService
{
    public IO<Unit> GoBackAsync(CancellationToken cancellationToken = default) =>
        mainThread.InvokeOnMainThread(() => shell.Navigation.PopAsync().WaitAsync(cancellationToken).ToUnit());

    public IO<Unit> GoToAsync(string path, CancellationToken cancellationToken = default) =>
        mainThread.InvokeOnMainThread(() => shell.GoToAsync(path).WaitAsync(cancellationToken));

    public IO<Unit> SetLogInAsRootAsync(bool animate = true,
        CancellationToken cancellationToken = default) =>
        mainThread.InvokeOnMainThread(() => shell.SetLogInAsRootAsync(animate, cancellationToken));

    public IO<Unit> SetMainPageAsRootAsync(bool animate = true,
        CancellationToken cancellationToken = default) =>
        mainThread.InvokeOnMainThread(() => shell.SetMainAsRootAsync(animate, cancellationToken));

    public IO<Unit> SetLogInAsRoot(bool animate = true,
        CancellationToken cancellationToken = default) =>
        mainThread.InvokeOnMainThread(() => shell.SetLogInAsRoot(animate, cancellationToken));

    public IO<Unit> SetMainPageAsRoot(bool animate = true,
        CancellationToken cancellationToken = default) =>
        mainThread.InvokeOnMainThread(() => shell.SetMainAsRoot(animate, cancellationToken));
}