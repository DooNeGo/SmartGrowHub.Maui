using SmartGrowHub.Maui.Application.Interfaces;
using SmartGrowHub.Maui.Infrastructure.Services.Extensions;
using System.Net.Http.Json;

namespace SmartGrowHub.Maui.Infrastructure.Services;

internal sealed class HttpService : IHttpService
{
    private readonly HttpClient _httpClient;

    public HttpService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://ftrjftdv-5116.euw.devtunnels.ms/api/");
        _httpClient.Timeout = TimeSpan.FromSeconds(10);
    }

    public Eff<Either<TError, TResponse>> GetAsync<TResponse, TError>(string urn, CancellationToken cancellationToken) =>
        liftEff(() => _httpClient.GetAsync(urn, cancellationToken))
            .Bind(response => response.Content
                .ReadJsonOrErrorAsync<TResponse, TError>(cancellationToken));

    public Eff<Either<TError, TResponse>> PostAsync<TRequest, TResponse, TError>(
        string urn, TRequest request, CancellationToken cancellationToken) =>
        liftEff(() => _httpClient.PostAsJsonAsync(urn, request, cancellationToken))
            .Bind(response => response.Content
                .ReadJsonOrErrorAsync<TResponse, TError>(cancellationToken));
}
