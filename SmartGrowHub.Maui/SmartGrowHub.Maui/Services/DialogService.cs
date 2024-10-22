using Mopups.Interfaces;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Maui.Application.Interfaces;
using SmartGrowHub.Maui.Popups;
using SmartGrowHub.Maui.Services.Extensions;

namespace SmartGrowHub.Maui.Services;

public sealed class DialogService : IDialogService
{
    private readonly LoadingPopup _loadingPopup;
    private readonly Atom<int> _loadingCount = Atom(0);
    private readonly IPopupNavigation _popupNavigation;

    public DialogService(IPopupNavigation popupNavigation)
    {
        _popupNavigation = popupNavigation;
        _loadingPopup = new(this);
    }

    public IO<bool> DisplayAlertAsync(string title, string message, string accept, string cancel,
        CancellationToken cancellationToken) =>
        App.Current!.Dispatcher.InvokeOnUiThreadIfNeeded(
            () => App.Current.MainPage!
                .DisplayAlert(title, message, accept, cancel)
                .WaitAsync(cancellationToken));

    public IO<Unit> DisplayAlertAsync(string title, string message, string cancel,
        CancellationToken cancellationToken) =>
        DisplayAlertAsync(title, message, null!, cancel, cancellationToken).Map(_ => unit);

    public IO<Unit> DisplayAlert(string title, string message, string cancel) =>
        lift(() => DisplayAlertAsync(title, message, cancel, CancellationToken.None)
            .RunAsync()
            .SafeFireAndForget());

    public IO<Unit> ShowLoadingAsync() =>
        _loadingCount
            .SwapIO(previous => previous + 1)
            .Bind(value => value is 1
                ? _popupNavigation.PushAsync(_loadingPopup).ToIO()
                : unitIO);

    public IO<Unit> PopAsync() =>
        _loadingCount
            .SwapIO(previous => previous > 0 ? previous - 1 : 0)
            .Bind(value => value is 0
                ? _popupNavigation.PopAsync().ToIO()
                : unitIO);

    public IO<Unit> PopAllAsync() =>
        _popupNavigation.PopAllAsync().ToIO()
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