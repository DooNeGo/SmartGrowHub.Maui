using Mopups.Interfaces;
using SmartGrowHub.Maui.Popups;
using SmartGrowHub.Maui.Services.Infrastructure;

namespace SmartGrowHub.Maui.Services.App;

public interface IDialogService
{
    IO<Unit> DisplayAlertAsync(string title, string message, string cancel);
    IO<bool> DisplayAlertAsync(string title, string message, string accept, string cancel);
    IO<Unit> ShowLoadingAsync();
    IO<Unit> PopAsync();
    IO<Unit> PopAllAsync();
}


public sealed class DialogService : IDialogService
{
    private readonly LoadingPopup _loadingPopup;
    private readonly IApplication _application;
    private readonly IPopupNavigation _popupNavigation;
    private readonly IMainThreadService _mainThread;

    public DialogService(IApplication application, IPopupNavigation popupNavigation, IMainThreadService mainThread)
    {
        _application = application;
        _popupNavigation = popupNavigation;
        _mainThread = mainThread;
        _loadingPopup = new LoadingPopup(this);
    }

    public IO<bool> DisplayAlertAsync(string title, string message, string accept, string cancel) =>
        _mainThread.InvokeOnMainThread(() => GetCurrentPage().DisplayAlert(title, message, accept, cancel));

    public IO<Unit> DisplayAlertAsync(string title, string message, string cancel) =>
        DisplayAlertAsync(title, message, null!, cancel).Map(_ => unit);

    public IO<Unit> ShowLoadingAsync() => liftIO(() => _popupNavigation.PushAsync(_loadingPopup));

    public IO<Unit> PopAsync() => liftIO(() => _popupNavigation.PopAsync());

    public IO<Unit> PopAllAsync() => liftIO(() => _popupNavigation.PopAllAsync());

    private Page GetCurrentPage() => _application.Windows[0].Content as Page ?? throw new InvalidOperationException();
}