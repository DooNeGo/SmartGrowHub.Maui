using LanguageExt.UnsafeValueAccess;
using MPowerKit;
using SmartGrowHub.Maui.Services.Infrastructure;

namespace SmartGrowHub.Maui.Services.App;

public interface INavigationService
{
    IO<Unit> NavigateAsync(string route, Option<IDictionary<string, object>> parameters = default, bool modal = false,
        bool animated = true);

    IO<Unit> GoBackAsync(Option<IDictionary<string, object>> parameters = default, bool modal = false,
        bool animated = true);
    
    NavigationBuilder CreateBuilder();
}

public sealed class MPowerKitNavigationService(
    MPowerKit.Navigation.Interfaces.INavigationService navigationService,
    IMainThread mainThread)
    : INavigationService
{
    public IO<Unit> NavigateAsync(string route, Option<IDictionary<string, object>> parameters = default,
        bool modal = false, bool animated = true)
    {
        NavigationParameters? navigationParameters = parameters.Map(ToNavigationParameters).ValueUnsafe();
        
        return mainThread.InvokeOnMainThread(() =>
            navigationService.NavigateAsync(route, navigationParameters, modal, animated)
                .AsTask().ToUnit());
    }

    public IO<Unit> GoBackAsync(Option<IDictionary<string, object>> parameters = default, bool modal = false,
        bool animated = true)
    {
        NavigationParameters? navigationParameters = parameters.Map(ToNavigationParameters).ValueUnsafe();
        
        return mainThread.InvokeOnMainThread(() =>
            navigationService.GoBackAsync(navigationParameters, modal, animated)
                .AsTask().ToUnit());
    }

    public NavigationBuilder CreateBuilder() => new(this);
    
    private static NavigationParameters ToNavigationParameters(IDictionary<string, object> parameters) =>
        new(parameters);
}