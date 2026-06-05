using SmartGrowHub.Maui.Services.App;

namespace SmartGrowHub.Maui.Services.Extensions;

public static class DialogExtensions
{
    extension(IDialogService dialogService)
    {
        public IO<Unit> DisplayAlert(string title, string message, string accept, string cancel) =>
            dialogService.DisplayAlert(title, message, accept, cancel).Fork().ToUnit();

        public IO<Unit> DisplayAlert(string title, string message,
            string cancel) => dialogService.DisplayAlert(title, message, cancel).Fork().ToUnit();
    }
}