namespace SmartGrowHub.Maui.Services.Extensions;

public static class ApiMockDiExtensions
{
    public static IServiceCollection AddApi<TService, TApi, TMock>(
        this IServiceCollection services, bool isDevelopment)
        where TService : class
        where TApi : class, TService
        where TMock : class, TService
        => isDevelopment
            ? services.AddScoped<TService, TMock>()
            : services.AddScoped<TService, TApi>();
}
