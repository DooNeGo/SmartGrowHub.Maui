using System.Net.Http.Headers;
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
    private readonly ITokenProvider _tokenProvider;

    public HttpService(HttpClient httpClient, ITokenProvider tokenProvider)
    {
        _httpClient = httpClient;
        _tokenProvider = tokenProvider;

        _httpClient.BaseAddress = new Uri("https://ftrjftdv-5116.euw.devtunnels.ms/api/");
        ConfigureAuthorizationAsync(_httpClient, CancellationToken.None).GetAwaiter().GetResult();
    }

    public TryOptionAsync<TResponse> GetAsync<TResponse>(string urn, CancellationToken cancellationToken) =>
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

    private TryOptionAsync<HttpClient> ConfigureAuthorizationAsync(HttpClient httpClient, CancellationToken cancellationToken) =>
        _tokenProvider.GetAsync(cancellationToken)
            .Map(token => new AuthenticationHeaderValue("Bearer", token))
            .Map(authentication => httpClient.DefaultRequestHeaders.Authorization = authentication)
            .Map(_ => httpClient);
    
    public void Dispose() => _httpClient.Dispose();
}
