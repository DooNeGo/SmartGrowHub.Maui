using SmartGrowHub.Maui.Popups;
using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Maui.Services.Infrastructure;

namespace SmartGrowHub.Maui.Services.App;

public interface IDialogService
{
    IO<Unit> DisplayAlertAsync(string title, string message, string cancel);
    IO<bool> DisplayAlertAsync(string title, string message, string accept, string cancel);
    IO<Unit> ShowLoadingAsync();
    IO<Unit> HideLoadingAsync();
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

    public IO<bool> DisplayAlertAsync(string title, string message, string accept, string cancel) =>
        IO.liftAsync(() => GetCurrentPage().DisplayAlert(title, message, accept, cancel)).Post();

    public IO<Unit> DisplayAlertAsync(string title, string message, string cancel) =>
        DisplayAlertAsync(title, message, null!, cancel).ToUnit();

    public IO<Unit> ShowLoadingAsync() => _popupNavigation.ShowPopupAsync(new LoadingPopup());

    public IO<Unit> HideLoadingAsync() => _popupNavigation.HidePopupAsync();

    private Page GetCurrentPage() => _application.Windows[0].Content as Page ?? throw new InvalidOperationException();
}