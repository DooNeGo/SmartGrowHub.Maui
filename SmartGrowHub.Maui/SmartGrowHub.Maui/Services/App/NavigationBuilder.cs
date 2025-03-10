using SmartGrowHub.Maui.Services.Extensions;

namespace SmartGrowHub.Maui.Services.App;

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

    public IO<Unit> NavigateAsync(bool modal = false, bool animated = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(_route, nameof(_route));
        return navigationService.NavigateAsync(_route, _parameters, modal, animated);
    }
}