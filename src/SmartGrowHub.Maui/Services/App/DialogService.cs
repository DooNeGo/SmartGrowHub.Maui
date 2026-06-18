using SmartGrowHub.Maui.Popups;
using SmartGrowHub.Maui.Services.Infrastructure;

namespace SmartGrowHub.Maui.Services.App;

public interface IDialogService
{
    Task DisplayAlertAsync(string title, string message, string cancel);
    Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel);
    ValueTask ShowLoadingAsync();
    ValueTask HideLoadingAsync();
}

public sealed class DialogService : IDialogService
{
    private readonly IApplication _application;
    private readonly IPopupNavigation _popupNavigation;

    public DialogService(IApplication application, IPopupNavigation popupNavigation)
    {
        _application = application;
        _popupNavigation = popupNavigation;
    }

    public Task<bool> DisplayAlertAsync(string title, string message, string accept, string cancel) =>
        GetCurrentPage().DisplayAlertAsync(title, message, accept, cancel);

    public Task DisplayAlertAsync(string title, string message, string cancel) =>
        GetCurrentPage().DisplayAlertAsync(title, message, cancel);

    public ValueTask ShowLoadingAsync() => _popupNavigation.ShowPopupAsync(new LoadingPopup());

    public ValueTask HideLoadingAsync() => _popupNavigation.HidePopupAsync();

    private Page GetCurrentPage() => _application.Windows[0].Content as Page ?? throw new InvalidOperationException();
}