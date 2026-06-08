using MPowerKit.Navigation.Utilities;
using SmartGrowHub.Maui.Features.ConfigureGrowHub.View;
using SmartGrowHub.Maui.Features.ConfigureGrowHub.ViewModel;
using SmartGrowHub.Maui.Features.Login.View;
using SmartGrowHub.Maui.Features.Login.ViewModel;
using SmartGrowHub.Maui.Features.Main.View;
using SmartGrowHub.Maui.Features.Main.ViewModel;

namespace SmartGrowHub.Maui.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddFeatures(this IServiceCollection services) =>
        services
            .RegisterForNavigation<StartPage, StartPageModel>(Routes.StartPage)
            .RegisterForNavigation<MainPage, MainPageModel>(Routes.MainPage)
            .RegisterForNavigation<RequestOtpToEmailPage, RequestOtpToEmailPageModel>(Routes.RequestOtpToEmailPage)
            .RegisterForNavigation<VerifyCodePage, VerifyCodePageModel>(Routes.VerifyCodePage)
            .RegisterForNavigation<LightControlPage, LightControlPageModel>(Routes.LightControlPage);
}
