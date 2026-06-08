namespace SmartGrowHub.Maui.Services.App;

public sealed class NavigationBuilder
{
    private readonly Dictionary<string, object> _parameters = [];
    private readonly string _route;
    private readonly INavigationService _navigationService;

    public NavigationBuilder(string route, INavigationService navigationService)
    {
        _route = route;
        _navigationService = navigationService;
    }

    public NavigationBuilder AddRouteParameter(string name, object value)
    {
        _parameters[name] = value;
        return this;
    }

    public IO<Unit> Navigate(bool modal = false, bool animated = true) =>
        string.IsNullOrWhiteSpace(_route)
            ? IO.fail<Unit>("Route cannot be null or whitespace")
            : _navigationService.Navigate(_route, _parameters, modal, animated);
}