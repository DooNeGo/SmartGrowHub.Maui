using Mopups.Interfaces;
using SmartGrowHub.Maui.Application.Interfaces;
using SmartGrowHub.Maui.Mopups;
using SmartGrowHub.Maui.Services.Extensions;

namespace SmartGrowHub.Maui.Services;

public sealed class DialogService(IPopupNavigation popupNavigation) : IDialogService
{
    private readonly LoadingMopup _loadingMopup = new();

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
        liftIO(() => popupNavigation
            .PushAsync(_loadingMopup)
            .ToUnit());

    public IO<Unit> PopAsync() =>
        liftIO(() => popupNavigation.PopupStack.Count > 0
            ? popupNavigation.PopAsync()
            : Task.CompletedTask);

    public IO<Unit> ShowLoading() =>
        lift(() => ShowLoadingAsync()
            .RunAsync()
            .SafeFireAndForget());

    public IO<Unit> Pop() =>
        lift(() => PopAsync()
            .RunAsync()
            .SafeFireAndForget());
}
