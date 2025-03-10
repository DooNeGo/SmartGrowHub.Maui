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

public sealed class MPowerKitPopupNavigation(
    INavigationPopupService popupService,
    IMainThread mainThread)
    : IPopupNavigation
{
    public IReadOnlyList<PopupPage> PopupStack => popupService.PopupStack;

    public IO<Unit> ShowPopupAsync(PopupPage page, bool animated = true) =>
        mainThread.InvokeOnMainThread(() => popupService.ShowPopupAsync(page, animated).AsTask());

    public IO<Unit> HidePopupAsync(bool animated = true) =>
        mainThread.InvokeOnMainThread(() => popupService.HidePopupAsync(animated).AsTask());

    public IO<Unit> HidePopupAsync(PopupPage page, bool animated = true) =>
        mainThread.InvokeOnMainThread(() => popupService.HidePopupAsync(page, animated).AsTask());
}