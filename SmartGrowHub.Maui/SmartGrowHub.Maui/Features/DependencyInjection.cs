using CommunityToolkit.Maui;
using SmartGrowHub.Maui.Features.ConfigureGrowHub.View;
using SmartGrowHub.Maui.Features.ConfigureGrowHub.ViewModel;
using SmartGrowHub.Maui.Features.Loading.View;
using SmartGrowHub.Maui.Features.LogIn.View;
using SmartGrowHub.Maui.Features.LogIn.ViewModel;
using SmartGrowHub.Maui.Features.Main.View;
using SmartGrowHub.Maui.Features.Main.ViewModel;

namespace SmartGrowHub.Maui.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddFeatures(this IServiceCollection services) =>
        services
            .AddTransient<LoadingPage>()
            .AddTransient<StartPage, StartPageModel>()
            .AddTransient<MainPage, MainPageModel>()
            .AddTransientWithShellRoute<LoginByEmailPage, LoginByEmailPageModel>(Routes.LoginByEmailPage)
            .AddTransientWithShellRoute<CheckCodePage, CheckCodePageModel>(Routes.CheckCodePage)
            .AddTransientWithShellRoute<LightControlPage, LightControlPageModel>(Routes.LightControlPage);
}
