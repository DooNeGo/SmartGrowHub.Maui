namespace SmartGrowHub.Maui.Application.Interfaces;

public interface IDialogService
{
    IO<Unit> DisplayAlertAsync(string title, string message, string cancel);
    IO<bool> DisplayAlertAsync(string title, string message, string accept, string cancel);
    IO<Unit> DisplayAlert(string title, string message, string cancel);
    IO<Unit> ShowLoadingAsync();
    IO<Unit> PopAsync();
    IO<Unit> PopAllAsync();
    IO<Unit> ShowLoading();
    IO<Unit> Pop();
}
