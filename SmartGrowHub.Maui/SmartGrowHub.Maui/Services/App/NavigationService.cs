using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Maui.Services.Infrastructure;

namespace SmartGrowHub.Maui.Services.App;

public interface INavigationService
{
    IO<Unit> InitializeRootPage(CancellationToken cancellationToken);
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
    public IO<Unit> InitializeRootPage(CancellationToken cancellationToken) =>
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
        from accessToken in secureStorage.GetAccessToken()
        from refreshToken in secureStorage.GetRefreshToken()
        select unit;
}

public sealed class NavigationBuilder(INavigationService navigationService)
{
    private Dictionary<string, object>? _parameters;
    private string _route = string.Empty;

    public NavigationBuilder AddRouteParameter(string name, object value)
    {
        _parameters ??= [];
        _parameters[name] = value;
        return this;
    }
    
    public NavigationBuilder SetRoute(string route)
    {
        _route = route;
        return this;
    }

    public NavigationBuilder AddQueryParameter(string name, string? value)
    {
        _route.AppendQueryParameter(name, value);
        return this;
    }

    public IO<Unit> NavigateAsync(bool animated = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(_route, nameof(_route));
        return navigationService.NavigateAsync(_route, _parameters, animated);
    }
}