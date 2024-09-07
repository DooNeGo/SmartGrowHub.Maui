using CommunityToolkit.Maui;
using SmartGrowHub.Maui.Features.LogIn.View;
using SmartGrowHub.Maui.Features.LogIn.ViewModel;
using SmartGrowHub.Maui.Features.Register.View;
using SmartGrowHub.Maui.Features.Register.ViewModel;
using SmartGrowHub.Maui.Features.Start.View;
using SmartGrowHub.Maui.Features.Start.ViewModel;
using SmartGrowHub.Maui.Services;

namespace SmartGrowHub.Maui;

internal static class DependencyInjection
{
    public static IServiceCollection AddUi(this IServiceCollection services) =>
        services
            .AddTransientWithShellRoute<LogInPage, LogInPageModel>(nameof(LogInPageModel))
            .AddTransientWithShellRoute<StartPage, StartPageModel>(nameof(StartPageModel))
            .AddTransientWithShellRoute<RegisterPage, RegisterPageModel>(nameof(RegisterPageModel));

    public static IServiceCollection AddServices(this IServiceCollection services) =>
        services
            .AddSingleton<IIdentityService, IdentityService>()
            .AddSingleton<Shell>(new AppShell())
            .AddSingleton(SecureStorage.Default);
}
