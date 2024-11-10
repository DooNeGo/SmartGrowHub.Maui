using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.Messaging;
using Mopups.Services;
using SmartGrowHub.Maui.Application.Interfaces;
using SmartGrowHub.Maui.Features.AppStart.View;
using SmartGrowHub.Maui.Features.AppStart.ViewModel;
using SmartGrowHub.Maui.Features.LogIn.View;
using SmartGrowHub.Maui.Features.LogIn.ViewModel;
using SmartGrowHub.Maui.Features.Main.View;
using SmartGrowHub.Maui.Features.Main.ViewModel;
using SmartGrowHub.Maui.Features.Register.View;
using SmartGrowHub.Maui.Features.Register.ViewModel;
using SmartGrowHub.Maui.Features.Start.View;
using SmartGrowHub.Maui.Features.Start.ViewModel;
using SmartGrowHub.Maui.Features.UserProfile.View;
using SmartGrowHub.Maui.Features.UserProfile.ViewModel;
using SmartGrowHub.Maui.Services;

namespace SmartGrowHub.Maui;

internal static class DependencyInjection
{
    public static IServiceCollection AddUi(this IServiceCollection services) =>
        services
            .AddTransientWithShellRoute<LogInPage, LogInPageModel>(nameof(LogInPageModel))
            .AddTransientWithShellRoute<StartPage, StartPageModel>(nameof(StartPageModel))
            .AddTransientWithShellRoute<RegisterPage, RegisterPageModel>(nameof(RegisterPageModel))
            .AddTransientWithShellRoute<MainPage, MainPageModel>(nameof(MainPageModel))
            .AddTransientWithShellRoute<UserProfilePage, UserProfilePageModel>(nameof(UserProfilePageModel))
            .AddTransientWithShellRoute<AppStartPage, AppStartPageModel>(nameof(AppStartPageModel));

    public static IServiceCollection AddServices(this IServiceCollection services) =>
        services
            .AddSingleton(MopupService.Instance)
            .AddSingleton(SecureStorage.Default)
            .AddSingleton<AppShell>()
            .AddSingleton<IMessenger>(WeakReferenceMessenger.Default)
            .AddSingleton<IDialogService, DialogService>()
            .AddSingleton<INavigationService, NavigationService>()
            .AddSingleton<IUserSessionService, UserSessionService>()
            .AddTransient<INoAuthorizeService, NoAuthorizeService>()
            .AddTransient<ILogInService, LogInService>()
            .AddTransient<ILogOutService, LogOutService>()
            .AddTransient<IRegisterService, RegisterService>()
            .AddTransient<IRefreshTokensService, RefreshTokensService>();
}
