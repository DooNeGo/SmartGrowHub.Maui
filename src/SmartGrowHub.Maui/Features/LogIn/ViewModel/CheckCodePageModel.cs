using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MPowerKit.Navigation.Awares;
using MPowerKit.Navigation.Interfaces;
using SmartGrowHub.Maui.Resources.Localization;
using SmartGrowHub.Maui.Services.App;
using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Maui.Services.Flow;
using INavigationService = SmartGrowHub.Maui.Services.App.INavigationService;

namespace SmartGrowHub.Maui.Features.LogIn.ViewModel;

public sealed partial class CheckCodePageModel : ObservableValidator, IPageLifecycleAware, IInitializeAware
{
    private readonly ILoginByEmailService _loginService;
    private readonly INavigationService _navigationService;
    private readonly IDialogService _dialogService;
    
    public CheckCodePageModel(
        ILoginByEmailService loginService,
        INavigationService navigationService,
        IDialogService dialogService)
    {
        _loginService = loginService;
        _navigationService = navigationService;
        _dialogService = dialogService;
    }

    [ObservableProperty] public partial string SentTo { get; set; } = string.Empty;
    
    [ObservableProperty]
    [NotifyDataErrorInfo]
    [Required(ErrorMessageResourceName = nameof(AppResources.RequiredField), ErrorMessageResourceType = typeof(AppResources))]
    public partial string Code { get; set; } = string.Empty;
    
    [ObservableProperty] public partial string CodeError { get; set; } = string.Empty;

    public void Initialize(INavigationParameters parameters) =>
        parameters
            .TryGetValue(nameof(SentTo))
            .Bind(obj => obj as string ?? Option<string>.None)
            .IfSome(value => SentTo = value);
    
    public void OnAppearing() => OnCodeChanged(Code);
    
    public void OnDisappearing() { }

    partial void OnCodeChanged(string value)
    {
        ClearErrors();
        ValidateProperty(value, nameof(Code));
        CodeError = GetErrors(nameof(Code)).FirstOrDefault()?.ErrorMessage ?? string.Empty;
    }

    [RelayCommand]
    private Task<Unit> GoBackAsync() => _navigationService.GoBackAsync().RunAsync().AsTask();

    [RelayCommand]
    private Task<Fin<Unit>> CheckCodeAsync(CancellationToken cancellationToken) => (
        from _1 in _dialogService.ShowLoading()
        from _2 in _loginService
            .CheckOtp(Code)
            .TapOnFail(DisplayError)
            .Finally(_dialogService.HideLoading())
        from _3 in _navigationService.NavigateAsync($"/{Routes.NavigationPage}/{Routes.MainPage}")
        select _3
    ).RunSafeAsync(EnvIO.New(token: cancellationToken)).AsTask();
    
    private IO<Unit> DisplayError(Error error) => IO.lift(() => CodeError = error.Message).ToUnit();
}