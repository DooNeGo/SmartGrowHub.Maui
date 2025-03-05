using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGrowHub.Maui.Base;
using SmartGrowHub.Maui.Resources.Localization;
using SmartGrowHub.Maui.Services.App;
using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Maui.Services.Flow;

namespace SmartGrowHub.Maui.Features.LogIn.ViewModel;

[QueryProperty("SentTo", "SentTo")]
public sealed partial class CheckCodePageModel(
    ILoginByEmailService loginByEmailService,
    INavigationService navigationService)
    : ObservableValidator, IPageLifecycleAware
{
    [ObservableProperty] public partial string SentTo { get; set; } = string.Empty;
    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessageResourceName = nameof(AppResources.RequiredField), ErrorMessageResourceType = typeof(AppResources))]
    public partial string Code { get; set; } = string.Empty;
    
    [ObservableProperty] public partial string CodeError { get; set; } = string.Empty;

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
            .CheckOtp(Code, cancellationToken)
            .IfFail(DisplayError)
            .RunSafeAsync()
            .AsTask();
    
    private IO<Unit> DisplayError(Error error) =>
        IO.lift(() => CodeError = error.Message).ToUnit();
}