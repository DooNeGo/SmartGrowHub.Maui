using LanguageExt.UnsafeValueAccess;
using MPowerKit;
using MPowerKit.Navigation;
using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Maui.Services.Infrastructure;

namespace SmartGrowHub.Maui.Services.App;

public interface INavigationService
{
    IO<Unit> InitializeRootPage();

    IO<Unit> NavigateAsync(string route, Option<IDictionary<string, object>> parameters = default, bool modal = false,
        bool animated = true);

    IO<Unit> GoBackAsync(Option<IDictionary<string, object>> parameters = default, bool modal = false,
        bool animated = true);
    
    NavigationBuilder CreateBuilder();
}

public sealed class MPowerKitNavigationService(MPowerKit.Navigation.Interfaces.INavigationService navigationService)
    : INavigationService
{
    public IO<Unit> InitializeRootPage()
    {
        throw new NotImplementedException();
    }

    public IO<Unit> NavigateAsync(string route, Option<IDictionary<string, object>> parameters = default,
        bool modal = false, bool animated = true)
    {
        NavigationParameters? s = parameters
            .Map(objects => new NavigationParameters(objects))
            .ValueUnsafe();

        return IO.liftAsync(async () =>
        {
            return await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                NavigationResult result = await navigationService.NavigateAsync(route, s, modal, animated);
                return Unit.Default;
            });
        });
    }

    public IO<Unit> GoBackAsync(Option<IDictionary<string, object>> parameters = default, bool modal = false,
        bool animated = true)
    {
        NavigationParameters? s = parameters
            .Map(dictionary => new NavigationParameters(dictionary))
            .ValueUnsafe();
        
        return IO.liftAsync(() =>
            MainThread.InvokeOnMainThreadAsync(() =>
                navigationService.GoBackAsync(s, modal, animated).AsTask())).ToUnit();
    }

    public NavigationBuilder CreateBuilder() => new(this);
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

    public IO<Unit> NavigateAsync(string route, Option<IDictionary<string, object>> parameters = default,
        bool modal = false, bool animated = true) =>
        mainThread.InvokeOnMainThread(() => parameters.Match(
            Some: dictionary => shell.GoToAsync(route, animated, dictionary),
            None: () => shell.GoToAsync(route, animated)));

    public IO<Unit> GoBackAsync(Option<IDictionary<string, object>> parameters = default, bool modal = false,
        bool animated = true) =>
        mainThread
            .InvokeOnMainThread(() => modal
                ? shell.Navigation.PopModalAsync(animated)
                : shell.Navigation.PopAsync(animated))
            .ToUnit();

    public NavigationBuilder CreateBuilder() => new(this);

    private OptionT<IO, Unit> AreAccessAndRefreshTokenExist() =>
        from _1 in secureStorage.GetAccessToken()
        from _2 in secureStorage.GetRefreshToken()
        select unit;
}