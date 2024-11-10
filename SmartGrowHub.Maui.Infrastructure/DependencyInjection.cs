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
            .AddTransient<TokenDelegatingHandler>()
            .AddTransient<AuthorizationErrorDelegatingHandler>()
            .AddHttpClients();

    private static IServiceCollection AddHttpClients(this IServiceCollection services)
    {
        services.AddHttpClient<IUserService, UserService>(DefaultConfiguration)
            .AddHttpMessageHandler<AuthorizationErrorDelegatingHandler>()
            .AddHttpMessageHandler<TokenDelegatingHandler>();

        services.AddHttpClient<IAuthService, AuthService>(DefaultConfiguration);

        return services;
    }
}
