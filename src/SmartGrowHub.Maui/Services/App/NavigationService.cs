using LanguageExt.UnsafeValueAccess;
using MPowerKit;
using MPowerKit.Navigation;
using Serilog;

namespace SmartGrowHub.Maui.Services.App;

public interface INavigationService
{
    IO<Unit> NavigateAsync(string route, Option<IDictionary<string, object>> parameters = default, bool modal = false,
        bool animated = true);

    IO<Unit> GoBackAsync(Option<IDictionary<string, object>> parameters = default, bool modal = false,
        bool animated = true);

    IO<Unit> GoBackToRootAsync(Option<IDictionary<string, object>> parameters = default, bool animated = true);
    
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

    public IO<Unit> NavigateAsync(string route, Option<IDictionary<string, object>> parameters = default,
        bool modal = false, bool animated = true) =>
        IO.liftVAsync(async () =>
        {
            NavigationParameters? navigationParameters = parameters.Map(ToNavigationParameters).ValueUnsafe();
            
            NavigationResult result = await _navigationService
                .NavigateAsync(route, navigationParameters, modal, animated)
                .ConfigureAwait(false);
            
            if (!result.Success) _logger.Fatal(result.Exception, "Navigation failed");
            
            return Unit.Default;
        });

    public IO<Unit> GoBackAsync(Option<IDictionary<string, object>> parameters = default, bool modal = false,
        bool animated = true) =>
        IO.liftVAsync(async () =>
        {
            NavigationParameters? navigationParameters = parameters.Map(ToNavigationParameters).ValueUnsafe();
            await _navigationService.GoBackAsync(navigationParameters, modal, animated).ConfigureAwait(false);
            return Unit.Default;
        });
    
    public IO<Unit> GoBackToRootAsync(Option<IDictionary<string, object>> parameters = default, bool animated = true) =>
        IO.liftVAsync(async () =>
        {
            NavigationParameters? navigationParameters = parameters.Map(ToNavigationParameters).ValueUnsafe();
            await _navigationService.GoBackToRootAsync(navigationParameters, animated).ConfigureAwait(false);
            return Unit.Default;
        });

    public NavigationBuilder CreateBuilder(string route) => new(route, this);
    
    private static NavigationParameters ToNavigationParameters(IDictionary<string, object> parameters) =>
        new(parameters);
}