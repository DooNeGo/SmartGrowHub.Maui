using AsyncAwaitBestPractices;
using Mopups.Interfaces;
using SmartGrowHub.Maui.Mopups;
using SmartGrowHub.Maui.Services.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace SmartGrowHub.Maui.Services;

public interface IDialogService
{
    Eff<Unit> DisplayAlertAsync(string title, string message, string cancel, CancellationToken cancellationToken);
    Eff<bool> DisplayAlertAsync(string title, string message, string accept, string cancel, CancellationToken cancellationToken);
    Unit DisplayAlert(string title, string message, string cancel);
    Eff<IAsyncDisposable> LoadingAsync();
    IDisposable Loading();
}

[SuppressMessage("Design", "CA1001:Типы, владеющие высвобождаемыми полями, должны быть высвобождаемыми", Justification = "<Ожидание>")]
public sealed class DialogService(IPopupNavigation popupNavigation) : IDialogService
{
    private readonly LoadingMopup _loadingMopup = new(popupNavigation);

    public Eff<bool> DisplayAlertAsync(string title, string message, string accept, string cancel,
        CancellationToken cancellationToken) =>
        liftEff(() => Application.Current!.Dispatcher.DispatchAsync(() =>
            Application.Current.MainPage!
                .DisplayAlert(title, message, accept, cancel)
                .WaitAsync(cancellationToken)));

    public Eff<Unit> DisplayAlertAsync(string title, string message, string cancel,
        CancellationToken cancellationToken) =>
        DisplayAlertAsync(title, message, null!, cancel, cancellationToken).Map(_ => unit);

    public Unit DisplayAlert(string title, string message, string cancel) =>
        DisplayAlertAsync(title, message, cancel, CancellationToken.None)
            .RunAsync().SafeFireAndForget();

    public Eff<IAsyncDisposable> LoadingAsync() =>
        liftEff(() => popupNavigation
            .PushAsync(_loadingMopup)
            .ToUnit()
            .Map(_ => (IAsyncDisposable)_loadingMopup));

    public IDisposable Loading()
    {
        LoadingAsync().RunAsync().SafeFireAndForget();
        return _loadingMopup;
    }
}
