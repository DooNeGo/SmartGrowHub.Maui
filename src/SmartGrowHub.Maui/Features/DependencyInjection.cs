using MPowerKit.Navigation.Utilities;
using SmartGrowHub.Maui.Features.GrowHub.Modules.View;
using SmartGrowHub.Maui.Features.GrowHub.Modules.ViewModel;
using SmartGrowHub.Maui.Features.Login.View;
using SmartGrowHub.Maui.Features.Login.ViewModel;
using SmartGrowHub.Maui.Features.Main.View;
using SmartGrowHub.Maui.Features.Main.ViewModel;
using SmartGrowHub.Maui.Popups;

namespace SmartGrowHub.Maui.Features;

public static class DependencyInjection
{
    public static IServiceCollection AddFeatures(this IServiceCollection services) =>
        services
            .RegisterForNavigation<StartPage, StartPageModel>(Routes.StartPage)
            .RegisterForNavigation<MainPage, MainPageModel>(Routes.MainPage)
            .RegisterForNavigation<RequestOtpToEmailPage, RequestOtpToEmailPageModel>(Routes.RequestOtpToEmailPage)
            .RegisterForNavigation<VerifyOtpPage, VerifyOtpPageModel>(Routes.VerifyOtpPage)
            .RegisterForNavigation<GrowHubModuleControlPage, GrowHubModuleControlPageModel>(Routes.GrowHubModuleControlPage)
            .RegisterForNavigation<ScheduleModePopupPage, ScheduleModePopupViewModel>(Routes.ScheduleModePopupPage);
}
