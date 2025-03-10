using SmartGrowHub.Maui.Popups;
using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Maui.Services.Infrastructure;

namespace SmartGrowHub.Maui.Services.App;

public interface IDialogService
{
    IO<Unit> DisplayAlertAsync(string title, string message, string cancel);
    IO<bool> DisplayAlertAsync(string title, string message, string accept, string cancel);
    IO<Unit> ShowLoadingAsync();
    IO<Unit> HideLoadingAsync();
}


public sealed class DialogService : IDialogService
{
    private readonly IApplication _application;
    private readonly IPopupNavigation _popupNavigation;
    private readonly IMainThread _mainThread;
    private readonly LoadingPopup _loadingPopup;

    public DialogService(IApplication application, IPopupNavigation popupNavigation, IMainThread mainThread)
    {
        _application = application;
        _popupNavigation = popupNavigation;
        _mainThread = mainThread;
        _loadingPopup = new LoadingPopup();
    }

    public IO<bool> DisplayAlertAsync(string title, string message, string accept, string cancel) =>
        _mainThread.InvokeOnMainThread(() => GetCurrentPage().DisplayAlert(title, message, accept, cancel));

    public IO<Unit> DisplayAlertAsync(string title, string message, string cancel) =>
        DisplayAlertAsync(title, message, null!, cancel).ToUnit();

    public IO<Unit> ShowLoadingAsync() => _popupNavigation.ShowPopupAsync(_loadingPopup);

    public IO<Unit> HideLoadingAsync() => _popupNavigation.HidePopupAsync(_loadingPopup);

    private Page GetCurrentPage() => _application.Windows[0].Content as Page ?? throw new InvalidOperationException();
}