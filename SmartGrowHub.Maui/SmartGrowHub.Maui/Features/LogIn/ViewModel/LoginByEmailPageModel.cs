using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGrowHub.Maui.Base;
using SmartGrowHub.Maui.Resources.Localization;
using SmartGrowHub.Maui.Services.Flow;

namespace SmartGrowHub.Maui.Features.LogIn.ViewModel;

public sealed partial class LoginByEmailPageModel(ILoginByEmailService logInService)
    : ObservableValidator, IPageLifecycleAware
{
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessageResourceName = nameof(AppResources.RequiredField), ErrorMessageResourceType = typeof(AppResources))]
    [EmailAddress(ErrorMessageResourceName = nameof(AppResources.InvalidEmailAddress), ErrorMessageResourceType = typeof(AppResources))]
    public partial string Email { get; set; } = string.Empty;

    [ObservableProperty] public partial string EmailError { get; set; } = string.Empty;

    public void OnAppearing() => OnEmailChanged(Email);

    public void OnDisappearing() { }

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
            .IfFail(DisplayError)
            .RunSafeAsync()
            .AsTask();

    [RelayCommand]
    private void OnGoBack() => SendOtpCommand.Cancel();

    private IO<Unit> DisplayError(Error error) =>
        IO.lift(() => EmailError = error.Message).Map(_ => unit);
}
