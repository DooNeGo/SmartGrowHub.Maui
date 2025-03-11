using LanguageExt.UnsafeValueAccess;
using MPowerKit;
using SmartGrowHub.Maui.Services.Extensions;

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

public sealed class MPowerKitNavigationService(MPowerKit.Navigation.Interfaces.INavigationService navigationService)
    : INavigationService
{
    public IO<Unit> NavigateAsync(string route, Option<IDictionary<string, object>> parameters = default,
        bool modal = false, bool animated = true)
    {
        NavigationParameters? navigationParameters = parameters.Map(ToNavigationParameters).ValueUnsafe();
        
        return IO.liftVAsync(() => navigationService
            .NavigateAsync(route, navigationParameters, modal, animated)).ToUnit();
    }

    public IO<Unit> GoBackAsync(Option<IDictionary<string, object>> parameters = default, bool modal = false,
        bool animated = true)
    {
        NavigationParameters? navigationParameters = parameters.Map(ToNavigationParameters).ValueUnsafe();
        return IO.liftVAsync(() => navigationService.GoBackAsync(navigationParameters, modal, animated)).ToUnit();
    }
    
    public IO<Unit> GoBackToRootAsync(Option<IDictionary<string, object>> parameters = default, bool animated = true)
    {
        NavigationParameters? navigationParameters = parameters.Map(ToNavigationParameters).ValueUnsafe();
        return IO.liftVAsync(() => navigationService.GoBackToRootAsync(navigationParameters, animated)).ToUnit();
    }

    public NavigationBuilder CreateBuilder(string route) => new(route, this);
    
    private static NavigationParameters ToNavigationParameters(IDictionary<string, object> parameters) =>
        new(parameters);
}