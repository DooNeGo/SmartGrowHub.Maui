using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MPowerKit.Navigation.Awares;
using Serilog;
using SmartGrowHub.Maui.Resources.Localization;
using SmartGrowHub.Maui.Services.App;

namespace SmartGrowHub.Maui.Features.Login.ViewModel;

public sealed partial class RequestOtpToEmailPageModel : ObservableValidator, IPageLifecycleAware
{
    private readonly IAuthService _authService;
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigationService;
    private readonly ILogger _logger;

    public RequestOtpToEmailPageModel(
        IAuthService authService,
        IDialogService dialogService,
        INavigationService navigationService,
        ILogger logger)
    {
        _authService = authService;
        _dialogService = dialogService;
        _navigationService = navigationService;
        _logger = logger;
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
    private async Task SendOtpAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _dialogService.ShowLoadingAsync();

            try
            {
                await _authService.RequestOtpToEmail(Email, cancellationToken);
            }
            catch (Exception ex)
            {
                EmailError = ex.Message;
            }
            finally
            {
                await _dialogService.HideLoadingAsync();
            }

            await _navigationService
                .CreateBuilder(Routes.VerifyOtpPage)
                .AddRouteParameter(nameof(VerifyOtpPageModel.SentTo), Email)
                .NavigateAsync().ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.Error(e, "Failed to send OTP");
        }
    }
}
