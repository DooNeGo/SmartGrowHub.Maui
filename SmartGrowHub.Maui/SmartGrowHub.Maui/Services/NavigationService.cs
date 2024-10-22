using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Maui.Application.Interfaces;
using SmartGrowHub.Maui.Services.Extensions;

namespace SmartGrowHub.Maui.Services;

public sealed class NavigationService(AppShell shell) : INavigationService
{
    private readonly IDispatcher _dispatcher = shell.Dispatcher;

    public IO<Unit> GoBackAsync(CancellationToken cancellationToken = default) =>
        _dispatcher.InvokeOnUiThreadIfNeeded(
            () => shell.Navigation.PopAsync()
                .WaitAsync(cancellationToken)
                .ToUnit());

    public IO<Unit> GoToAsync(string path, CancellationToken cancellationToken = default) =>
        shell.GoToAsync(path).ToIO();

    public IO<Unit> SetLogInAsRoot() =>
        shell.SetLogInAsRoot();

    public IO<Unit> SetMainPageAsRoot() =>
        shell.SetMainAsRoot();
}