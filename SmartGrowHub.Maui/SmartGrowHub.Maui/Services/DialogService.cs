using Mopups.Interfaces;
using SmartGrowHub.Maui.Mopups;
using SmartGrowHub.Maui.Services.Extensions;

namespace SmartGrowHub.Maui.Services;

public interface IDialogService
{
    IO<Unit> DisplayAlertAsync(string title, string message, string cancel, CancellationToken cancellationToken);
    IO<bool> DisplayAlertAsync(string title, string message, string accept, string cancel, CancellationToken cancellationToken);
    IO<Unit> DisplayAlert(string title, string message, string cancel);
    IO<Unit> ShowLoadingAsync();
    IO<Unit> HideLoadingAsync();
    IO<Unit> ShowLoading();
    IO<Unit> HideLoading();
}

public sealed class DialogService(IPopupNavigation popupNavigation) : IDialogService
{
    private readonly LoadingMopup _loadingMopup = new();

    public IO<bool> DisplayAlertAsync(string title, string message, string accept, string cancel,
        CancellationToken cancellationToken) =>
        liftIO(() => Application.Current!.Dispatcher.InvokeOnUiThreadIfNeeded(
            () => Application.Current.MainPage!
                .DisplayAlert(title, message, accept, cancel)
                .WaitAsync(cancellationToken)));

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

    public IO<Unit> HideLoadingAsync() =>
        liftIO(() => popupNavigation
            .PopAsync()
            .ToUnit());

    public IO<Unit> ShowLoading() =>
        lift(() => ShowLoadingAsync()
            .RunAsync()
            .SafeFireAndForget());

    public IO<Unit> HideLoading() =>
        lift(() => HideLoadingAsync()
            .RunAsync()
            .SafeFireAndForget());
}
