using Mopups.Interfaces;
using SmartGrowHub.Maui.Popups;
using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Maui.Services.Infrastructure;

namespace SmartGrowHub.Maui.Services.App;

public interface IDialogService
{
    IO<Unit> DisplayAlertAsync(string title, string message, string cancel);
    IO<bool> DisplayAlertAsync(string title, string message, string accept, string cancel);
    IO<Unit> DisplayAlert(string title, string message, string cancel);
    IO<Unit> ShowLoadingAsync();
    IO<Unit> PopAsync();
    IO<Unit> PopAllAsync();
    IO<Unit> ShowLoading();
    IO<Unit> Pop();
}


public sealed class DialogService : IDialogService
{
    private readonly LoadingPopup _loadingPopup;
    private readonly Atom<int> _loadingCount = Atom(0);
    private readonly IPopupNavigation _popupNavigation;
    private readonly IMainThreadService _mainThread;

    public DialogService(IPopupNavigation popupNavigation, IMainThreadService mainThread)
    {
        _popupNavigation = popupNavigation;
        _mainThread = mainThread;
        _loadingPopup = new LoadingPopup(this);
    }

    public IO<bool> DisplayAlertAsync(string title, string message, string accept, string cancel) =>
        _mainThread.InvokeOnMainThread(() => DisplayAlertInternal(title, message, accept, cancel));

    public IO<Unit> DisplayAlert(string title, string message, string cancel) =>
        _mainThread.BeginInvokeOnMainThread(() =>
            DisplayAlertInternal(title, message, null!, cancel).SafeFireAndForget());

    private static Task<bool> DisplayAlertInternal(string title, string message, string accept, string cancel) =>
        Application.Current!.Windows[0].Page!.DisplayAlert(title, message, accept, cancel);

    public IO<Unit> DisplayAlertAsync(string title, string message, string cancel) =>
        DisplayAlertAsync(title, message, null!, cancel).Map(_ => unit);

    public IO<Unit> ShowLoadingAsync() =>
        _popupNavigation.PopupStack.Contains(_loadingPopup)
            ? unitIO : liftIO(() => _popupNavigation.PushAsync(_loadingPopup));

    public IO<Unit> PopAsync() =>
        _loadingCount
            .SwapIO(previous => previous > 0 ? previous - 1 : 0)
            .Bind(value => value is 0
                ? liftIO(() => _popupNavigation.PopAsync())
                : unitIO);

    public IO<Unit> PopAllAsync() =>
        liftIO(() => _popupNavigation.PopAllAsync())
            .Bind(_ => _loadingCount
                .SwapIO(_ => 0)
                .Map(_ => unit));

    public IO<Unit> ShowLoading() =>
        lift(() => ShowLoadingAsync()
            .RunAsync()
            .SafeFireAndForget());

    public IO<Unit> Pop() =>
        lift(() => PopAsync()
            .RunAsync()
            .SafeFireAndForget());
}