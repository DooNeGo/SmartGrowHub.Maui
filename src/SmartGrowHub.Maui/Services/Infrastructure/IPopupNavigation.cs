using MPowerKit.Navigation.Interfaces;
using MPowerKit.Navigation.Popups;
using MPowerKit.Popups;

namespace SmartGrowHub.Maui.Services.Infrastructure;

public interface IPopupNavigation
{
    IReadOnlyList<PopupPage> PopupStack { get; }

    ValueTask<PopupResult> ShowAwaitablePopupAsync(string route, INavigationParameters? parameters = null, bool animated = true);
    ValueTask ShowPopupAsync(PopupPage page, bool animated = true);
    ValueTask HidePopupAsync(bool animated = true);
    ValueTask HidePopupAsync(PopupPage page, bool animated = true);
}

public sealed class MPowerKitPopupNavigation(
    INavigationPopupService navigationPopupService,
    IPopupNavigationService popupNavigationService) : IPopupNavigation
{
    public IReadOnlyList<PopupPage> PopupStack => navigationPopupService.PopupStack;

    public ValueTask<PopupResult> ShowAwaitablePopupAsync(string route, INavigationParameters? parameters = null,
        bool animated = true) =>
        popupNavigationService.ShowAwaitablePopupAsync(route, parameters, animated);
    
    public ValueTask ShowPopupAsync(PopupPage page, bool animated = true) =>
        navigationPopupService.ShowPopupAsync(page, animated);

    public ValueTask HidePopupAsync(bool animated = true) =>
        navigationPopupService.HidePopupAsync(animated);

    public ValueTask HidePopupAsync(PopupPage page, bool animated = true) =>
        navigationPopupService.HidePopupAsync(page, animated);
}