using Mopups.Interfaces;
using SmartGrowHub.Maui.Mopups;

namespace SmartGrowHub.Maui.Services;

public interface IDialogService
{
    Task<Unit> DisplayAlertAsync(string title, string message, string cancel, CancellationToken cancellationToken);
    Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel, CancellationToken cancellationToken);
    Task<IAsyncDisposable> Loading();
}

public sealed class DialogService(IPopupNavigation popupNavigation) : IDialogService
{
    public Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel,
        CancellationToken cancellationToken) =>
        Application.Current!.Dispatcher.DispatchAsync(() => Application.Current.MainPage!
            .DisplayAlert(title, message, accept, cancel)
            .WaitAsync(cancellationToken));

    public Task<Unit> DisplayAlertAsync(string title, string message, string cancel,
        CancellationToken cancellationToken) =>
        DisplayAlertAsync(title, message, null!, cancel, cancellationToken).ToUnit();

    public Task<IAsyncDisposable> Loading() =>
        Id(new LoadingMopup(popupNavigation))
            .Map(mopup => popupNavigation
                .PushAsync(mopup).ToUnit()
                .Map(_ => (IAsyncDisposable)mopup))
            .Value;
}