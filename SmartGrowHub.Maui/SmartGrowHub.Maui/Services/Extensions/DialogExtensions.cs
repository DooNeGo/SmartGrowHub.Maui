using SmartGrowHub.Maui.Services.App;

namespace SmartGrowHub.Maui.Services.Extensions;

public static class DialogExtensions
{
    public static IO<Unit> DisplayAlert(
        this IDialogService dialogService, string title, string message, string accept, string cancel) =>
        IO.lift(() => dialogService.DisplayAlertAsync(title, message, accept, cancel).RunAsync().SafeFireAndForget());
    
    public static IO<Unit> DisplayAlert(
        this IDialogService dialogService, string title, string message, string cancel) =>
        IO.lift(() => dialogService.DisplayAlertAsync(title, message, cancel).RunAsync().SafeFireAndForget());

    public static IO<Unit> Pop(this IDialogService dialogService) =>
        IO.lift(() => dialogService.PopAsync().RunAsync().SafeFireAndForget());
}