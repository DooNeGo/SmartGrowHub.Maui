using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Options;
using Mopups.Services;
using SmartGrowHub.Maui.Services.Api;
using SmartGrowHub.Maui.Services.App;
using SmartGrowHub.Maui.Services.DelegatingHandlers;
using SmartGrowHub.Maui.Services.Flow;
using SmartGrowHub.Maui.Services.Infrastructure;

namespace SmartGrowHub.Maui.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services) =>
        services
            .AddApiServices()
            .AddAppServices()
            .AddFlowServices()
            .AddInfrastructureServices();

    private static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services
            .AddTransient<IAuthService, AuthService>()
            .AddTransient<TokenDelegatingHandler>()
            .AddTransient<AuthorizationErrorDelegatingHandler>();
        
        services.AddHttpClient(Options.DefaultName, DefaultConfiguration)
            .AddHttpMessageHandler<TokenDelegatingHandler>()
            .AddHttpMessageHandler<AuthorizationErrorDelegatingHandler>();

        services.AddHttpClient<IAuthService, AuthService>(DefaultConfiguration);

        return services;

        static void DefaultConfiguration(HttpClient client)
        {
            client.BaseAddress = new Uri("https://ftrjftdv-7260.euw.devtunnels.ms");
            client.Timeout = TimeSpan.FromSeconds(40);
        }
    }

    private static IServiceCollection AddAppServices(this IServiceCollection services) =>
        services
            .AddSingleton<IDialogService, DialogService>()
            .AddSingleton<INavigationService, NavigationService>()
            .AddSingleton(MopupService.Instance)
            .AddSingleton(WeakReferenceMessenger.Default);

    private static IServiceCollection AddFlowServices(this IServiceCollection services) =>
        services
            .AddTransient<IAuthorizationErrorHandler, AuthorizationErrorHandler>()
            .AddTransient<ILoginByEmailService, LoginByEmailService>()
            .AddTransient<ILogoutService, LogoutService>()
            .AddTransient<IRefreshTokensService, RefreshTokensService>();

    private static IServiceCollection AddInfrastructureServices(this IServiceCollection services) =>
        services
            .AddSingleton<IMainThreadService, MainThreadService>()
            .AddSingleton<ITokensStorage, TokensStorage>()
            .AddSingleton(SecureStorage.Default);
}
