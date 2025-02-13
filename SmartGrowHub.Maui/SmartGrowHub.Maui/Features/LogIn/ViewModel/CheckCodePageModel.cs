using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGrowHub.Maui.Base;
using SmartGrowHub.Maui.Resources.Localization;
using SmartGrowHub.Maui.Services.App;
using SmartGrowHub.Maui.Services.Flow;

namespace SmartGrowHub.Maui.Features.LogIn.ViewModel;

[QueryProperty(nameof(SentTo), nameof(SentTo))]
public sealed partial class CheckCodePageModel(
    ILoginByEmailService loginByEmailService,
    INavigationService navigationService)
    : ObservableValidator, IPageLifecycleAware
{
    [ObservableProperty] private string _sentTo = string.Empty;
    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessageResourceName = nameof(AppResources.RequiredField), ErrorMessageResourceType = typeof(AppResources))]
    private string _code = string.Empty;
    
    [ObservableProperty] private string _codeError = string.Empty;

    public void OnAppearing() => OnCodeChanged(Code);

    partial void OnCodeChanged(string value)
    {
        ClearErrors();
        ValidateProperty(value, nameof(Code));
        CodeError = GetErrors(nameof(Code)).FirstOrDefault()?.ErrorMessage ?? string.Empty;
    }

    [RelayCommand]
    private Task<Unit> GoBackAsync(CancellationToken cancellationToken) =>
        navigationService.GoBackAsync().RunAsync().AsTask();

    [RelayCommand]
    private Task<Fin<Unit>> CheckCodeAsync(CancellationToken cancellationToken) =>
        loginByEmailService
            .CheckOtp(int.Parse(Code), cancellationToken)
            .IfFailEff(DisplayError)
            .RunAsync();
    
    private Eff<Unit> DisplayError(Error error) =>
        liftEff(() => CodeError = error.Message).Map(_ => unit);
}