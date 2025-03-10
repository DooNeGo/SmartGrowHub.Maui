using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MPowerKit.Navigation.Awares;
using SmartGrowHub.Maui.Base;
using SmartGrowHub.Maui.Resources.Localization;
using SmartGrowHub.Maui.Services.App;
using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Maui.Services.Flow;

namespace SmartGrowHub.Maui.Features.LogIn.ViewModel;

[QueryProperty("SentTo", "SentTo")]
public sealed partial class CheckCodePageModel(
    ILoginByEmailService loginService,
    INavigationService navigationService,
    IDialogService dialogService)
    : ObservableValidator, IPageLifecycleAware
{
    [ObservableProperty] public partial string SentTo { get; set; } = string.Empty;
    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessageResourceName = nameof(AppResources.RequiredField), ErrorMessageResourceType = typeof(AppResources))]
    public partial string Code { get; set; } = string.Empty;
    
    [ObservableProperty] public partial string CodeError { get; set; } = string.Empty;

    public void OnAppearing() => OnCodeChanged(Code);
    
    public void OnDisappearing() { }

    partial void OnCodeChanged(string value)
    {
        ClearErrors();
        ValidateProperty(value, nameof(Code));
        CodeError = GetErrors(nameof(Code)).FirstOrDefault()?.ErrorMessage ?? string.Empty;
    }

    [RelayCommand]
    private Task<Unit> GoBackAsync() =>
        navigationService.GoBackAsync().RunAsync().AsTask();

    [RelayCommand]
    private Task<Fin<Unit>> CheckCodeAsync(CancellationToken cancellationToken) => (
        from _1 in dialogService.ShowLoadingAsync()
        from _2 in loginService
            .CheckOtp(Code, cancellationToken)
            .IfFail(DisplayError)
        from _3 in dialogService.HideLoadingAsync()
        from _4 in navigationService.NavigateAsync($"/NavigationPage/{Routes.MainPage}")
        select _4
    ).RunSafeAsync().AsTask();
    
    private IO<Unit> DisplayError(Error error) =>
        IO.lift(() => CodeError = error.Message).ToUnit();
}