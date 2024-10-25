using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Maui.Application.Interfaces;
using SmartGrowHub.Maui.Infrastructure.Services;
using SmartGrowHub.Maui.Infrastructure.Services.DelegatingHandlers;

namespace SmartGrowHub.Maui.Infrastructure;

public static class DependencyInjection
{
    private static readonly Action<HttpClient> DefaultConfiguration = client =>
    {
        client.BaseAddress = new Uri("https://ftrjftdv-5116.euw.devtunnels.ms/api/");
        client.Timeout = TimeSpan.FromSeconds(40);
    };

    public static IServiceCollection AddInfrastructure(this IServiceCollection services) =>
        services
            .AddSingleton<IMessenger>(WeakReferenceMessenger.Default)
            .AddTransient<IAuthService, AuthService>()
            .AddTransient<IUserService, UserService>()
            .AddTransient<TokenDelegatingHandler>()
            .AddTransient<NoAuthorizeDelegatingHandler>()
            .AddSingleton<IUserSessionProvider, UserSessionProvider>()
            .AddTransient<ILogInService, LogInService>()
            .AddTransient<ILogOutService, LogOutService>()
            .AddTransient<IRegisterService, RegisterService>()
            .AddTransient<IRefreshTokensService, RefreshTokensService>()
            .AddHttpClients();

    private static IServiceCollection AddHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient<IUserService, UserService>(DefaultConfiguration)
            .AddHttpMessageHandler<NoAuthorizeDelegatingHandler>()
            .AddHttpMessageHandler<TokenDelegatingHandler>();

        services.AddHttpClient<IAuthService, AuthService>(DefaultConfiguration);

        return services;
    }
}
