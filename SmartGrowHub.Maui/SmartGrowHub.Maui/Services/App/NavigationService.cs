using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Maui.Services.Infrastructure;

namespace SmartGrowHub.Maui.Services.App;

public interface INavigationService
{
    IO<Unit> InitializeRootPage();
    IO<Unit> NavigateAsync(string route, IDictionary<string, object>? routeParameters = null, bool animated = true);
    IO<Unit> GoBackAsync(bool animated = true);
    NavigationBuilder CreateBuilder();
}

public sealed class ShellNavigationService(
    ISecureStorage secureStorage,
    IMainThreadService mainThread,
    AppShell shell)
    : INavigationService
{
    public IO<Unit> InitializeRootPage() =>
        AreAccessAndRefreshTokenExist()
            .Match(Some: _ => Routes.MainPage, None: () => Routes.StartPage).As()
            .Bind(route => NavigateAsync($"//{route}"));

    public IO<Unit> NavigateAsync(string route, IDictionary<string, object>? routeParameters = null,
        bool animated = true) =>
        mainThread.InvokeOnMainThread(() => routeParameters is null
            ? shell.GoToAsync(route, animated)
            : shell.GoToAsync(route, animated, routeParameters));

    public IO<Unit> GoBackAsync(bool animated = true) =>
        mainThread.InvokeOnMainThread(() => shell.Navigation.PopAsync(animated)).Map(_ => unit);

    public NavigationBuilder CreateBuilder() => new(this);

    private OptionT<IO, Unit> AreAccessAndRefreshTokenExist() =>
        from _1 in secureStorage.GetAccessToken()
        from _2 in secureStorage.GetRefreshToken()
        select unit;
}