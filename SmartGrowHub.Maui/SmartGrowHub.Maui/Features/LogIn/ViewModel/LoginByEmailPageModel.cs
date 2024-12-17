using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGrowHub.Maui.Base;
using SmartGrowHub.Maui.Resources.Localization;
using SmartGrowHub.Maui.Services.Flow;

namespace SmartGrowHub.Maui.Features.LogIn.ViewModel;

public sealed partial class LoginByEmailPageModel(ILoginByEmailService logInService)
    : ObservableValidator, IOnAppearedAware
{
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessageResourceName = nameof(AppResources.RequiredField), ErrorMessageResourceType = typeof(AppResources))]
    [EmailAddress(ErrorMessageResourceName = nameof(AppResources.InvalidEmailAddress), ErrorMessageResourceType = typeof(AppResources))]
    private string _email = string.Empty;

    [ObservableProperty] private string _emailError = string.Empty;

    public void OnAppeared() => OnEmailChanged(Email);

    partial void OnEmailChanged(string value)
    {
        ClearErrors();
        ValidateProperty(value, nameof(Email));
        EmailError = GetErrors(nameof(Email)).FirstOrDefault()?.ErrorMessage ?? string.Empty;
    }

    [RelayCommand]
    private Task<Fin<Unit>> SendOtpAsync(CancellationToken cancellationToken) =>
        logInService
            .SendOtpToEmail(Email, cancellationToken)
            .IfFailEff(DisplayError)
            .RunAsync();

    [RelayCommand]
    private void OnGoBack() => SendOtpCommand.Cancel();

    private Eff<Unit> DisplayError(Error error) =>
        liftEff(() => EmailError = error.Message).Map(_ => unit);
}
