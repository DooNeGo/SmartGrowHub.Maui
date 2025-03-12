using MPowerKit.Navigation.Interfaces;
using MPowerKit.Popups;

namespace SmartGrowHub.Maui.Services.Infrastructure;

public interface IPopupNavigation
{
    IReadOnlyList<PopupPage> PopupStack { get; }

    IO<Unit> ShowPopupAsync(PopupPage page, bool animated = true);
    IO<Unit> HidePopupAsync(bool animated = true);
    IO<Unit> HidePopupAsync(PopupPage page, bool animated = true);
}

public sealed class MPowerKitPopupNavigation(INavigationPopupService popupService) : IPopupNavigation
{
    public IReadOnlyList<PopupPage> PopupStack => popupService.PopupStack;

    public IO<Unit> ShowPopupAsync(PopupPage page, bool animated = true) =>
        IO.liftVAsync(() => popupService.ShowPopupAsync(page, animated).ToUnit());

    public IO<Unit> HidePopupAsync(bool animated = true) =>
        IO.liftVAsync(() => popupService.HidePopupAsync(animated).ToUnit());

    public IO<Unit> HidePopupAsync(PopupPage page, bool animated = true) =>
        IO.liftVAsync(() => popupService.HidePopupAsync(page, animated).ToUnit());
}