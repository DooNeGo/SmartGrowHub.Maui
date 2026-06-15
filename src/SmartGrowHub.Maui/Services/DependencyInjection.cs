using System.Text.Json;
using SmartGrowHub.Maui.Services.Api;
using SmartGrowHub.Maui.Services.App;
using SmartGrowHub.Maui.Services.DelegatingHandlers;
using SmartGrowHub.Maui.Services.Infrastructure;
using SmartGrowHub.Shared.SerializerContext;
using TimeProvider = SmartGrowHub.Maui.Services.Infrastructure.TimeProvider;

namespace SmartGrowHub.Maui.Services;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddServices() =>
            services
                .AddApiServices()
                .AddAppServices()
                .AddInfrastructureServices();

        private IServiceCollection AddApiServices()
        {
            services
                .AddSingleton<IAuthApi, AuthApi>()
                .AddSingleton<IGrowHubApi, GrowHubApi>()
                .AddTransient<TokenDelegatingHandler>()
                .AddTransient<UnauthorizedDelegatingHandler>();

            services.AddHttpClient(string.Empty, ConfigureHttpClient)
                .AddHttpMessageHandler<UnauthorizedDelegatingHandler>()
                .AddHttpMessageHandler<TokenDelegatingHandler>();

            services.AddHttpClient(nameof(IAuthApi), ConfigureHttpClient);

            return services;

            static void ConfigureHttpClient(HttpClient client)
            {
                client.BaseAddress =
                    //new Uri("https://rants-unheard-seizing.ngrok-free.dev");
                    new Uri("http://192.168.0.111:8080");
                client.Timeout = TimeSpan.FromSeconds(15);
            }
        }

        private IServiceCollection AddAppServices() =>
            services
                .AddSingleton<IDialogService, DialogService>()
                .AddSingleton<IPopupNavigation, MPowerKitPopupNavigation>()
                .AddScoped<INavigationService, MPowerKitNavigationService>()
                .AddSingleton<IAuthService, AuthService>();

        private IServiceCollection AddInfrastructureServices() =>
            services
                .AddTransient<IHttpService, HttpService>()
                .AddSingleton<ITimeProvider, TimeProvider>()
                .AddSingleton<IMainThread, DefaultMainThread>()
                .AddSingleton(SecureStorage.Default)
                .AddSingleton<IJsonSerializer, SystemJsonSerializer>(_ =>
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        RespectNullableAnnotations = true,
                        RespectRequiredConstructorParameters = true
                    };
                    options.TypeInfoResolverChain.Add(SmartGrowHubSerializerContext.Default);
                    return new SystemJsonSerializer(options);
                });
    }
}
