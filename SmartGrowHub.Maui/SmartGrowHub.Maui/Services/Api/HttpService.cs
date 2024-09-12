using System.Net.Http.Json;

namespace SmartGrowHub.Maui.Services.Api;

public interface IHttpService
{
    TryOptionAsync<TResponse> GetAsync<TResponse>(string urn, CancellationToken cancellationToken);
    TryOptionAsync<TResponse> PostAsync<TRequest, TResponse>(string urn, TRequest request, CancellationToken cancellationToken);
}

public sealed class HttpService : IHttpService, IDisposable
{
    private readonly HttpClient _httpClient;

    public HttpService(HttpClient httpClient)
    {
        _httpClient = httpClient;

        _httpClient.BaseAddress = new Uri("https://ftrjftdv-5116.euw.devtunnels.ms/api/");
    }

    public TryOptionAsync<TResponse> GetAsync<TResponse>(string urn,
        CancellationToken cancellationToken) =>
        TryOptionAsync(() => _httpClient
            .GetAsync(urn, cancellationToken)
            .MapAsync(response => response.Content
                .ReadFromJsonAsync<TResponse>(cancellationToken)
                .Map(Optional)));

    public TryOptionAsync<TResponse> PostAsync<TRequest, TResponse>(
        string urn, TRequest request, CancellationToken cancellationToken) =>
        TryOptionAsync(() => _httpClient
            .PostAsJsonAsync(urn, request, cancellationToken)
            .MapAsync(response => response.Content
                .ReadFromJsonAsync<TResponse>(cancellationToken)
                .Map(Optional)));

    public void Dispose() => _httpClient.Dispose();
}