namespace SmartGrowHub.Maui.Services;

public interface IDialogService
{
    Task DisplayAlertAsync(string title, string message, string cancel, CancellationToken cancellationToken);
    Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel, CancellationToken cancellationToken);
}

public sealed class DialogService : IDialogService
{
    public Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel,
        CancellationToken cancellationToken) =>
        Application.Current!.Dispatcher.DispatchAsync(() => Application.Current.MainPage!
            .DisplayAlert(title, message, accept, cancel)
            .WaitAsync(cancellationToken));

    public Task DisplayAlertAsync(string title, string message, string cancel,
        CancellationToken cancellationToken) =>
        DisplayAlertAsync(title, message, null!, cancel, cancellationToken);
}