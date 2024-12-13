using CommunityToolkit.Maui;
using SmartGrowHub.Maui.Features.Loading.View;
using SmartGrowHub.Maui.Features.Loading.ViewModel;
using SmartGrowHub.Maui.Features.LogIn.View;
using SmartGrowHub.Maui.Features.LogIn.ViewModel;
using SmartGrowHub.Maui.Features.Main.View;
using SmartGrowHub.Maui.Features.Main.ViewModel;
using SmartGrowHub.Maui.Features.Start.View;
using SmartGrowHub.Maui.Features.Start.ViewModel;

namespace SmartGrowHub.Maui.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddFeatures(this IServiceCollection services) =>
        services
            .AddSingleton(new AppShell())
            .AddTransientWithShellRoute<LoadingPage, LoadingPageModel>(nameof(LoadingPageModel))
            .AddTransientWithShellRoute<StartPage, StartPageModel>(nameof(StartPageModel))
            .AddTransientWithShellRoute<MainPage, MainPageModel>(nameof(MainPageModel))
            .AddTransientWithShellRoute<LoginByEmailPage, LoginByEmailPageModel>(nameof(LoginByEmailPageModel))
            .AddTransientWithShellRoute<CheckCodePage, CheckCodePageModel>(nameof(CheckCodePageModel));
}
