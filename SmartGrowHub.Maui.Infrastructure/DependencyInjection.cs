using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Maui.Application.Interfaces;
using SmartGrowHub.Maui.Infrastructure.Services.APIs;
using SmartGrowHub.Maui.Infrastructure.Services.DelegatingHandlers;
using SmartGrowHub.Maui.Infrastructure.Services.Mocks;

namespace SmartGrowHub.Maui.Infrastructure;

public static class DependencyInjection
{
    private static readonly Action<HttpClient> DefaultConfiguration = client =>
    {
        client.BaseAddress = new Uri("https://ftrjftdv-5116.euw.devtunnels.ms/api/");
        client.Timeout = TimeSpan.FromSeconds(40);
    };

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, bool isDevelopment) =>
        isDevelopment ? services.AddMockServices() : services.AddHttpServices();

    private static IServiceCollection AddHttpServices(this IServiceCollection services)
    {
        services
            .AddTransient<TokenDelegatingHandler>()
            .AddTransient<AuthorizationErrorDelegatingHandler>()
            .AddTransient<IUserService, UserService>();
        
        services
            .AddHttpClient(Options.DefaultName, DefaultConfiguration)
            .AddHttpMessageHandler<AuthorizationErrorDelegatingHandler>()
            .AddHttpMessageHandler<TokenDelegatingHandler>();
        
        services.AddHttpClient<IAuthService, AuthService>(DefaultConfiguration);

        return services;
    }

    private static IServiceCollection AddMockServices(this IServiceCollection services) =>
        services
            .AddSingleton<IAuthService, MockAuthService>()
            .AddSingleton<IUserService, MockUserService>();
}
