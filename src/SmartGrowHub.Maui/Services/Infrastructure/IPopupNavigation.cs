using MPowerKit.Navigation.Interfaces;
using MPowerKit.Navigation.Popups;
using MPowerKit.Popups;

namespace SmartGrowHub.Maui.Services.Infrastructure;

public interface IPopupNavigation
{
    IReadOnlyList<PopupPage> PopupStack { get; }

    IO<PopupResult> ShowAwaitablePopup(string route, INavigationParameters? parameters = null, bool animated = true);
    IO<Unit> ShowPopup(PopupPage page, bool animated = true);
    IO<Unit> HidePopup(bool animated = true);
    IO<Unit> HidePopup(PopupPage page, bool animated = true);
}

public sealed class MPowerKitPopupNavigation(
    INavigationPopupService navigationPopupService,
    IPopupNavigationService popupNavigationService) : IPopupNavigation
{
    public IReadOnlyList<PopupPage> PopupStack => navigationPopupService.PopupStack;

    public IO<PopupResult> ShowAwaitablePopup(string route, INavigationParameters? parameters = null,
        bool animated = true) =>
        IO.liftVAsync(() => popupNavigationService.ShowAwaitablePopupAsync(route, parameters, animated));
    
    public IO<Unit> ShowPopup(PopupPage page, bool animated = true) =>
        IO.liftVAsync(() => navigationPopupService.ShowPopupAsync(page, animated).ToUnit());

    public IO<Unit> HidePopup(bool animated = true) =>
        IO.liftVAsync(() => navigationPopupService.HidePopupAsync(animated).ToUnit());

    public IO<Unit> HidePopup(PopupPage page, bool animated = true) =>
        IO.liftVAsync(() => navigationPopupService.HidePopupAsync(page, animated).ToUnit());
}