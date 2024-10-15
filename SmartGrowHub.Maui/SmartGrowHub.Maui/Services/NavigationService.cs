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
        _dispatcher.InvokeOnUiThreadIfNeeded(() =>
            shell.GoToAsync(path)
                .WaitAsync(cancellationToken)
                .ToUnit());

    public IO<Unit> GoToLogIn() =>
        shell.SetLogInAsRoot();

    public IO<Unit> GoToMain() =>
        shell.SetMainAsRoot();
}