using MPowerKit.Navigation.Interfaces;
using MPowerKit.Popups;

namespace SmartGrowHub.Maui.Services.Infrastructure;

public interface IPopupNavigation
{
    IReadOnlyList<PopupPage> PopupStack { get; }

    IO<Unit> ShowPopup(PopupPage page, bool animated = true);
    IO<Unit> HidePopup(bool animated = true);
    IO<Unit> HidePopup(PopupPage page, bool animated = true);
}

public sealed class MPowerKitPopupNavigation(INavigationPopupService popupService) : IPopupNavigation
{
    public IReadOnlyList<PopupPage> PopupStack => popupService.PopupStack;

    public IO<Unit> ShowPopup(PopupPage page, bool animated = true) =>
        IO.liftVAsync(() => popupService.ShowPopupAsync(page, animated).ToUnit());

    public IO<Unit> HidePopup(bool animated = true) =>
        IO.liftVAsync(() => popupService.HidePopupAsync(animated).ToUnit());

    public IO<Unit> HidePopup(PopupPage page, bool animated = true) =>
        IO.liftVAsync(() => popupService.HidePopupAsync(page, animated).ToUnit());
}