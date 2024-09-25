using CommunityToolkit.Maui;
using Mopups.Services;
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
using SmartGrowHub.Maui.Services.Abstractions;
using SmartGrowHub.Maui.Services.Api;

namespace SmartGrowHub.Maui;

internal static class DependencyInjection
{
    public static IServiceCollection AddUi(this IServiceCollection services) =>
        services
            .AddTransientWithShellRoute<LogInPage, LogInPageModel>(nameof(LogInPageModel))
            .AddTransientWithShellRoute<StartPage, StartPageModel>(nameof(StartPageModel))
            .AddTransientWithShellRoute<RegisterPage, RegisterPageModel>(nameof(RegisterPageModel))
            .AddTransientWithShellRoute<MainPage, MainPageModel>(nameof(MainPageModel))
            .AddTransientWithShellRoute<UserProfilePage, UserProfilePageModel>(nameof(UserProfilePageModel));

    public static IServiceCollection AddServices(this IServiceCollection services) =>
        services
            .AddSingleton<IUserService, UserService>()
            .AddSingleton<AppShell>()
            .AddSingleton(SecureStorage.Default)
            .AddSingleton<ISecureStorageService, SecureStorageService>()
            .AddSingleton<IUserSessionProvider, UserSessionProvider>()
            .AddSingleton<IDialogService, DialogService>()
            .AddSingleton(MopupService.Instance)
            .AddSingleton<INavigationService, NavigationService>();

    public static IServiceCollection AddHttpServices(this IServiceCollection services)
    {
        services
            .AddSingleton<IAuthService, AuthService>()
            .AddSingleton<IHttpService, HttpService>()
            .AddHttpClient<IHttpService, HttpService>();
        return services;
    }
}
