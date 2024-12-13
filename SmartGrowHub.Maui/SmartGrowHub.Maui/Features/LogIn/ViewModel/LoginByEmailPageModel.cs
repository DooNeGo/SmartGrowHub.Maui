using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGrowHub.Maui.Resources.Localization;
using SmartGrowHub.Maui.Services.App;
using SmartGrowHub.Maui.Services.Flow;

namespace SmartGrowHub.Maui.Features.LogIn.ViewModel;

public sealed partial class LoginByEmailPageModel(
    ILoginByEmailService logInService,
    IDialogService dialogService)
    : ObservableValidator
{
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required]
    [EmailAddress]
    private string _email = string.Empty;

    [ObservableProperty] private string _emailError = string.Empty;

    [RelayCommand]
    private Task SendOtpAsync(CancellationToken cancellationToken) =>
        logInService.SendOtpToEmail(Email, cancellationToken)
            .IfFailEff(error => DisplayError(error))
            .RunAsync();

    [RelayCommand]
    private void OnGoBack() => SendOtpCommand.Cancel();

    private IO<Unit> DisplayError(Error error) =>
        dialogService.DisplayAlert(
            AppResources.Authorization,
            error.Message,
            AppResources.Ok);
}
