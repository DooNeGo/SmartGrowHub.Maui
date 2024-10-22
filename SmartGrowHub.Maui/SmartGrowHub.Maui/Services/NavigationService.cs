using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Maui.Application.Interfaces;

namespace SmartGrowHub.Maui.Services;

public sealed class NavigationService(AppShell shell) : INavigationService
{
    public IO<Unit> GoBackAsync(CancellationToken cancellationToken = default) =>
        shell.GoToAsync("..").WaitAsync(cancellationToken).ToIO();

    public IO<Unit> GoToAsync(string path, CancellationToken cancellationToken = default) =>
        shell.GoToAsync(path).WaitAsync(cancellationToken).ToIO();

    public IO<Unit> SetLogInAsRoot(bool animate = true,
        CancellationToken cancellationToken = default) =>
        shell.SetLogInAsRoot(animate, cancellationToken);

    public IO<Unit> SetMainPageAsRoot(bool animate = true,
        CancellationToken cancellationToken = default) =>
        shell.SetMainAsRoot(animate, cancellationToken);
}