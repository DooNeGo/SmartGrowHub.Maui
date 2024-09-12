using AsyncAwaitBestPractices;
using Mopups.Interfaces;
using SmartGrowHub.Maui.Mopups;
using SmartGrowHub.Maui.Services.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace SmartGrowHub.Maui.Services;

public interface IDialogService
{
    Task<Unit> DisplayAlertAsync(string title, string message, string cancel, CancellationToken cancellationToken);
    Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel, CancellationToken cancellationToken);
    Unit DisplayAlert(string title, string message, string cancel);
    Task<IAsyncDisposable> LoadingAsync();
    IDisposable Loading();
}

[SuppressMessage("Design", "CA1001:Типы, владеющие высвобождаемыми полями, должны быть высвобождаемыми", Justification = "<Ожидание>")]
public sealed class DialogService(IPopupNavigation popupNavigation) : IDialogService
{
    private readonly LoadingMopup _loadingMopup = new(popupNavigation);

    public Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel,
        CancellationToken cancellationToken) =>
        Application.Current!.Dispatcher.DispatchAsync(() => Application.Current.MainPage!
            .DisplayAlert(title, message, accept, cancel)
            .WaitAsync(cancellationToken));

    public Task<Unit> DisplayAlertAsync(string title, string message, string cancel,
        CancellationToken cancellationToken) =>
        DisplayAlertAsync(title, message, null!, cancel, cancellationToken).ToUnit();

    public Unit DisplayAlert(string title, string message, string cancel) =>
        DisplayAlertAsync(title, message, cancel, CancellationToken.None).SafeFireAndForget();

    public Task<IAsyncDisposable> LoadingAsync() =>
        popupNavigation
            .PushAsync(_loadingMopup).ToUnit()
            .Map(_ => (IAsyncDisposable)_loadingMopup);

    public IDisposable Loading()
    {
        popupNavigation.PushAsync(_loadingMopup).SafeFireAndForget();
        return _loadingMopup;
    }
}