using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using SmartGrowHub.Application.Services;
using SmartGrowHub.Maui.Application.Interfaces;
using SmartGrowHub.Maui.Infrastructure.Services;
using SmartGrowHub.Maui.Infrastructure.Services.DelegatingHandlers;

namespace SmartGrowHub.Maui.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services
            .AddSingleton<IMessenger>(WeakReferenceMessenger.Default)
            .AddTransient<IAuthService, AuthService>()
            .AddTransient<IUserService, UserService>()
            .AddSingleton<IUserSessionProvider, UserSessionProvider>()
            .AddHttpClient<IHttpService, HttpService>();

        services.AddHttpClient<IUserService, UserService>()
            .AddHttpMessageHandler<TokenDelegatingHandler>();

        return services;
    }
}
