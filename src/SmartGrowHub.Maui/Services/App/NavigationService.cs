using MPowerKit;
using MPowerKit.Navigation;
using Serilog;

namespace SmartGrowHub.Maui.Services.App;

public interface INavigationService
{
    Task NavigateAsync(string route, IDictionary<string, object>? parameters = null, bool modal = false,
        bool animated = true);

    Task GoBackAsync(IDictionary<string, object>? parameters = null, bool modal = false, bool animated = true);
    
    NavigationBuilder CreateBuilder(string route);
}

public sealed class MPowerKitNavigationService : INavigationService
{
    private readonly MPowerKit.Navigation.Interfaces.INavigationService _navigationService;
    private readonly ILogger _logger;

    public MPowerKitNavigationService(
        MPowerKit.Navigation.Interfaces.INavigationService navigationService,
        ILogger logger)
    {
        _navigationService = navigationService;
        _logger = logger;
    }

    public async Task NavigateAsync(string route, IDictionary<string, object>? parameters = null,
        bool modal = false, bool animated = true)
    {
        NavigationParameters? navigationParameters = parameters is not null 
            ? new NavigationParameters(parameters) 
            : null;

        NavigationResult result = await _navigationService
            .NavigateAsync(route, navigationParameters, modal, animated)
            .ConfigureAwait(false);

        if (!result.Success) 
            _logger.Fatal(result.Exception, "Navigation failed");
    }

    public async Task GoBackAsync(IDictionary<string, object>? parameters = null, bool modal = false,
        bool animated = true)
    {
        NavigationParameters? navigationParameters = parameters is not null 
            ? new NavigationParameters(parameters) 
            : null;
        await _navigationService.GoBackAsync(navigationParameters, modal, animated).ConfigureAwait(false);
    }

    public NavigationBuilder CreateBuilder(string route) => new(route, this);
}