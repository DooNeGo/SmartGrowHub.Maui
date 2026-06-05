using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MPowerKit.Navigation.Awares;
using SmartGrowHub.Maui.Resources.Localization;
using SmartGrowHub.Maui.Services.App;
using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Maui.Services.Flow;

namespace SmartGrowHub.Maui.Features.LogIn.ViewModel;

public sealed partial class LoginByEmailPageModel(
    ILoginByEmailService logInService,
    IDialogService dialogService,
    INavigationService navigationService)
    : ObservableValidator, IPageLifecycleAware
{
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessageResourceName = nameof(AppResources.RequiredField), ErrorMessageResourceType = typeof(AppResources))]
    [EmailAddress(ErrorMessageResourceName = nameof(AppResources.InvalidEmailAddress), ErrorMessageResourceType = typeof(AppResources))]
    public partial string Email { get; set; } = string.Empty;

    [ObservableProperty] public partial string EmailError { get; private set; } = string.Empty;

    public void OnAppearing() => OnEmailChanged(Email);

    public void OnDisappearing() => SendOtpCommand.Cancel();

    partial void OnEmailChanged(string value)
    {
        ClearErrors();
        ValidateProperty(value, nameof(Email));
        EmailError = GetErrors(nameof(Email)).FirstOrDefault()?.ErrorMessage ?? string.Empty;
    }

    [RelayCommand]
    private Task<Fin<Unit>> SendOtpAsync(CancellationToken cancellationToken) => (
        from _1 in dialogService.ShowLoadingAsync()
        from _2 in logInService
            .SendOtpToEmail(Email, cancellationToken)
            .TapOnFail(DisplayError)
            .Finally(dialogService.HideLoading())
        from _3 in navigationService
            .CreateBuilder(Routes.CheckCodePage)
            .AddRouteParameter(nameof(CheckCodePageModel.SentTo), Email)
            .NavigateAsync()
        select _3
    ).RunSafeAsync().AsTask();

    private IO<Unit> DisplayError(Error error) =>
        IO.lift(() => EmailError = error.Message).ToUnit();
}
