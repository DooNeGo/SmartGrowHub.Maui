using Mopups.Interfaces;
using MPowerKit.Popups.Interfaces;
using SmartGrowHub.Maui.Popups;
using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Maui.Services.Infrastructure;

namespace SmartGrowHub.Maui.Services.App;

public interface IDialogService
{
    IO<Unit> DisplayAlertAsync(string title, string message, string cancel);
    IO<bool> DisplayAlertAsync(string title, string message, string accept, string cancel);
    IO<Unit> ShowLoadingAsync();
    IO<Unit> PopAsync();
}


public sealed class DialogService : IDialogService
{
    private readonly IApplication _application;
    private readonly IPopupService _popupNavigation;
    private readonly IMainThreadService _mainThread;

    public DialogService(IApplication application, IPopupService popupNavigation, IMainThreadService mainThread)
    {
        _application = application;
        _popupNavigation = popupNavigation;
        _mainThread = mainThread;
    }

    public IO<bool> DisplayAlertAsync(string title, string message, string accept, string cancel) =>
        _mainThread.InvokeOnMainThread(() => GetCurrentPage().DisplayAlert(title, message, accept, cancel));

    public IO<Unit> DisplayAlertAsync(string title, string message, string cancel) =>
        DisplayAlertAsync(title, message, null!, cancel).ToUnit();

    public IO<Unit> ShowLoadingAsync() =>
        IO.liftVAsync(() => _popupNavigation.ShowPopupAsync(new LoadingPopup(this)).ToUnit());

    public IO<Unit> PopAsync() => IO.liftVAsync(() => _popupNavigation.HidePopupAsync().ToUnit());

    private Page GetCurrentPage() => _application.Windows[0].Content as Page ?? throw new InvalidOperationException();
}