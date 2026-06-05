using SmartGrowHub.Maui.Services.App;

namespace SmartGrowHub.Maui.Services.Extensions;

public static class DialogExtensions
{
    public static IO<Unit> DisplayAlert(this IDialogService dialogService, string title, string message, string accept,
        string cancel) =>
        dialogService.DisplayAlertAsync(title, message, accept, cancel).Fork().ToUnit();

    public static IO<Unit> DisplayAlert(this IDialogService dialogService, string title, string message,
        string cancel) =>
        dialogService.DisplayAlertAsync(title, message, cancel).Fork().ToUnit();

    public static IO<Unit> HideLoading(this IDialogService dialogService) =>
        dialogService.HideLoadingAsync().Fork().ToUnit();
}