using CommunityToolkit.Maui;
using SmartGrowHub.Maui.Features.ConfigureGrowHub.View;
using SmartGrowHub.Maui.Features.ConfigureGrowHub.ViewModel;
using SmartGrowHub.Maui.Features.Loading.View;
using SmartGrowHub.Maui.Features.Loading.ViewModel;
using SmartGrowHub.Maui.Features.LogIn.View;
using SmartGrowHub.Maui.Features.LogIn.ViewModel;
using SmartGrowHub.Maui.Features.Main.View;
using SmartGrowHub.Maui.Features.Main.ViewModel;

namespace SmartGrowHub.Maui.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddFeatures(this IServiceCollection services) =>
        services
            .AddTransientWithShellRoute<LoadingPage, LoadingPageModel>(nameof(LoadingPageModel))
            .AddTransientWithShellRoute<StartPage, StartPageModel>(nameof(StartPageModel))
            .AddTransientWithShellRoute<MainPage, MainPageModel>(nameof(MainPageModel))
            .AddTransientWithShellRoute<LoginByEmailPage, LoginByEmailPageModel>(nameof(LoginByEmailPageModel))
            .AddTransientWithShellRoute<CheckCodePage, CheckCodePageModel>(nameof(CheckCodePageModel))
            .AddTransientWithShellRoute<EnvironmentControlPage, EnvironmentControlPageModel>(nameof(EnvironmentControlPageModel))
            .AddTransientWithShellRoute<LightControlPage, LightControlPageModel>(nameof(LightControlPageModel));
}
