namespace SmartGrowHub.Maui.Services.App;

public sealed class NavigationBuilder(string route, INavigationService navigationService)
{
    private readonly Dictionary<string, object> _parameters = [];

    public NavigationBuilder AddRouteParameter(string name, object value)
    {
        _parameters[name] = value;
        return this;
    }

    public IO<Unit> NavigateAsync(bool modal = false, bool animated = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(route, nameof(route));
        return navigationService.NavigateAsync(route, _parameters, modal, animated);
    }
}