using SmartGrowHub.Maui.Popups;
using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Maui.Services.Infrastructure;

namespace SmartGrowHub.Maui.Services.App;

public interface IDialogService
{
    IO<Unit> DisplayAlert(string title, string message, string cancel);
    IO<bool> DisplayAlert(string title, string message, string accept, string cancel);
    IO<Unit> ShowLoading();
    IO<Unit> HideLoading();
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

    public IO<bool> DisplayAlert(string title, string message, string accept, string cancel) =>
        IO.liftAsync(() => GetCurrentPage().DisplayAlertAsync(title, message, accept, cancel));

    public IO<Unit> DisplayAlert(string title, string message, string cancel) =>
        DisplayAlert(title, message, null!, cancel).ToUnit();

    public IO<Unit> ShowLoading() => _popupNavigation.ShowPopup(new LoadingPopup());

    public IO<Unit> HideLoading() => _popupNavigation.HidePopup();

    private Page GetCurrentPage() => _application.Windows[0].Content as Page ?? throw new InvalidOperationException();
}