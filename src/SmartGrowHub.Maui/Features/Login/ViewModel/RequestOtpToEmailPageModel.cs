using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MPowerKit.Navigation.Awares;
using SmartGrowHub.Maui.Resources.Localization;
using SmartGrowHub.Maui.Services.App;
using SmartGrowHub.Maui.Services.Extensions;

namespace SmartGrowHub.Maui.Features.Login.ViewModel;

public sealed partial class RequestOtpToEmailPageModel : ObservableValidator, IPageLifecycleAware
{
    private readonly IAuthService _authService;
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigationService;
    
    public RequestOtpToEmailPageModel(
        IAuthService authService,
        IDialogService dialogService,
        INavigationService navigationService)
    {
        _authService = authService;
        _dialogService = dialogService;
        _navigationService = navigationService;
    }

    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessageResourceName = nameof(AppResources.RequiredField), ErrorMessageResourceType = typeof(AppResources))]
    [EmailAddress(ErrorMessageResourceName = nameof(AppResources.InvalidEmailAddress), ErrorMessageResourceType = typeof(AppResources))]
    public partial string Email { get; set; } = string.Empty;

    [ObservableProperty] public partial string EmailError { get; private set; } = string.Empty;

    public void OnAppearing() => OnEmailChanged(Email);

    public void OnDisappearing() { }

    partial void OnEmailChanged(string value)
    {
        ClearErrors();
        ValidateProperty(value, nameof(Email));
        EmailError = GetErrors(nameof(Email)).FirstOrDefault()?.ErrorMessage ?? string.Empty;
    }

    [RelayCommand]
    private Task<Fin<Unit>> SendOtpAsync(CancellationToken cancellationToken) => (
        from _1 in _dialogService.ShowLoading()
        from _2 in _authService
            .RequestOtpToEmail(Email)
            .TapOnFail(DisplayError)
            .Finally(_dialogService.HideLoading())
        from _3 in _navigationService
            .CreateBuilder(Routes.VerifyCodePage)
            .AddRouteParameter(nameof(VerifyCodePageModel.SentTo), Email)
            .NavigateAsync()
        select _3
    ).RunSafeAsync(EnvIO.New(token: cancellationToken)).AsTask();

    private IO<Unit> DisplayError(Error error) =>
        IO.lift(() => EmailError = error.Message).ToUnit();
}
