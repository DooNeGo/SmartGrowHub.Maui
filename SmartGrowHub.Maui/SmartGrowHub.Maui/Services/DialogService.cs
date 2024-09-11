using AsyncAwaitBestPractices;
using Mopups.Interfaces;
using SmartGrowHub.Maui.Mopups;
using SmartGrowHub.Maui.Services.Extensions;

namespace SmartGrowHub.Maui.Services;

public interface IDialogService
{
    Task<Unit> DisplayAlertAsync(string title, string message, string cancel, CancellationToken cancellationToken);
    Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel, CancellationToken cancellationToken);
    Unit DisplayAlert(string title, string message, string cancel, CancellationToken cancellationToken);
    Task<IAsyncDisposable> LoadingAsync();
    IDisposable Loading();
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

    public Unit DisplayAlert(string title, string message, string cancel, CancellationToken cancellationToken) =>
        DisplayAlertAsync(title, message, cancel, cancellationToken).SafeFireAndForget();

    public Task<IAsyncDisposable> LoadingAsync() =>
        Id(new LoadingMopup(popupNavigation))
            .Map(mopup => popupNavigation
                .PushAsync(mopup).ToUnit()
                .Map(_ => (IAsyncDisposable)mopup))
            .Value;

    public IDisposable Loading()
    {
        var mopup = new LoadingMopup(popupNavigation);
        popupNavigation.PushAsync(mopup).SafeFireAndForget();
        return mopup;
    }
}