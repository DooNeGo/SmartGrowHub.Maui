using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MPowerKit.Navigation.Awares;
using MPowerKit.Navigation.Interfaces;
using Serilog;
using SmartGrowHub.Maui.Resources.Localization;
using SmartGrowHub.Maui.Services.App;
using INavigationService = SmartGrowHub.Maui.Services.App.INavigationService;

namespace SmartGrowHub.Maui.Features.Login.ViewModel;

public sealed partial class VerifyOtpPageModel : ObservableValidator, IPageLifecycleAware, IInitializeAware
{
    private readonly IAuthService _authService;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    private readonly ILogger _logger;

    public VerifyOtpPageModel(
        IAuthService authService,
        INavigationService navigationService,
        IDialogService dialogService,
        ILogger logger)
    {
        _authService = authService;
        _navigationService = navigationService;
        _dialogService = dialogService;
        _logger = logger;
    }

    [ObservableProperty] public partial string SentTo { get; set; } = string.Empty;
    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessageResourceName = nameof(AppResources.RequiredField), ErrorMessageResourceType = typeof(AppResources))]
    public partial string Code { get; set; } = string.Empty;
    
    [ObservableProperty] public partial string CodeError { get; set; } = string.Empty;

    public void Initialize(INavigationParameters parameters)
    {
        var sentTo = parameters.GetValue<string?>(nameof(SentTo));
        if (sentTo is not null) SentTo = sentTo;
    }
    
    public void OnAppearing() => OnCodeChanged(Code);
    
    public void OnDisappearing() { }

    partial void OnCodeChanged(string value)
    {
        ClearErrors();
        ValidateProperty(value, nameof(Code));
        CodeError = GetErrors(nameof(Code)).FirstOrDefault()?.ErrorMessage ?? string.Empty;
    }

    [RelayCommand]
    private Task GoBackAsync() => _navigationService.GoBackAsync();

    [RelayCommand]
    private async Task CheckCodeAsync(CancellationToken cancellationToken)
    {
        try
        {
            await _dialogService.ShowLoadingAsync();

            try
            {
                await _authService.VerifyOtp(Code, cancellationToken);
            }
            catch (Exception ex)
            {
                CodeError = ex.Message;
            }
            finally
            {
                await _dialogService.HideLoadingAsync();
            }

            await _navigationService.NavigateAsync($"/{Routes.NavigationPage}/{Routes.MainPage}").ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.Error(e, "Failed to verify OTP");
        }
    }
}