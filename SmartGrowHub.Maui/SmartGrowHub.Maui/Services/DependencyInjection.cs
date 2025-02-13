using System.Text.Json;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Options;
using Mopups.Services;
using SmartGrowHub.Maui.Services.Api;
using SmartGrowHub.Maui.Services.App;
using SmartGrowHub.Maui.Services.DelegatingHandlers;
using SmartGrowHub.Maui.Services.Flow;
using SmartGrowHub.Maui.Services.Infrastructure;
using SmartGrowHub.Shared.SerializerContext;
using TimeProvider = SmartGrowHub.Maui.Services.Infrastructure.TimeProvider;

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
            .AddSingleton<IAuthService, AuthService>()
            .AddSingleton<IGrowHubService, GrowHubService>()
            .AddTransient<TokenDelegatingHandler>()
            .AddTransient<AuthorizationErrorDelegatingHandler>();
        
        services.AddHttpClient(Options.DefaultName, ConfigureHttpClient)
            .AddHttpMessageHandler<TokenDelegatingHandler>()
            .AddHttpMessageHandler<AuthorizationErrorDelegatingHandler>();

        services.AddHttpClient(nameof(IAuthService), ConfigureHttpClient);

        return services;

        static void ConfigureHttpClient(HttpClient client)
        {
            client.BaseAddress = new Uri("https://h8sqq0wq-7260.euw.devtunnels.ms");
            client.Timeout = TimeSpan.FromSeconds(40);
        }
    }

    private static IServiceCollection AddAppServices(this IServiceCollection services) =>
        services
            .AddSingleton<IDialogService, DialogService>()
            .AddSingleton<INavigationService, ShellNavigationService>()
            .AddSingleton(MopupService.Instance)
            .AddSingleton(WeakReferenceMessenger.Default);

    private static IServiceCollection AddFlowServices(this IServiceCollection services) =>
        services
            .AddTransient<IAuthorizationErrorHandler, AuthorizationErrorHandler>()
            .AddTransient<ILoginByEmailService, LoginByEmailService>()
            .AddTransient<ILogoutService, LogoutService>();

    private static IServiceCollection AddInfrastructureServices(this IServiceCollection services) =>
        services
            .AddTransient<HttpService>()
            .AddSingleton<ITimeProvider, TimeProvider>()
            .AddSingleton<IMainThreadService, MainThreadService>()
            .AddSingleton(SecureStorage.Default)
            .AddSingleton<IJsonSerializerService, JsonSerializerService>(_ =>
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                options.TypeInfoResolverChain.Add(SmartGrowHubSerializerContext.Default);
                return new JsonSerializerService(options);
            });
}
