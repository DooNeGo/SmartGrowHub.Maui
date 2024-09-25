using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace SmartGrowHub.Maui.Services.Api;

public interface IHttpService : IDisposable
{
    Eff<Option<TResponse>> GetAsync<TResponse>(string urn, CancellationToken cancellationToken);
    Eff<Option<TResponse>> PostAsync<TRequest, TResponse>(string urn, TRequest request, CancellationToken cancellationToken);
}

public sealed class HttpService : IHttpService
{
    private readonly HttpClient _httpClient;
    private readonly IUserSessionProvider _sessionProvider;

    public HttpService(HttpClient httpClient, IUserSessionProvider sessionProvider)
    {
        _httpClient = httpClient;
        _sessionProvider = sessionProvider;

        _httpClient.BaseAddress = new Uri("https://ftrjftdv-5116.euw.devtunnels.ms/api/");
    }

    public Eff<Option<TResponse>> GetAsync<TResponse>(string urn, CancellationToken cancellationToken) =>
        liftEff(() => _httpClient
            .GetAsync(urn, cancellationToken)
            .Bind(response => response.Content
                .ReadFromJsonAsync<TResponse>(cancellationToken)
                .Map(Optional)));

    public Eff<Option<TResponse>> PostAsync<TRequest, TResponse>(
        string urn, TRequest request, CancellationToken cancellationToken) =>
        liftEff(() => _httpClient
            .PostAsJsonAsync(urn, request, cancellationToken)
            .Bind(response => response.Content
                .ReadFromJsonAsync<TResponse>(cancellationToken)
                .Map(Optional)));

    public void Dispose() => _httpClient.Dispose();

    private Eff<HttpClient> ConfigureAuthentication(HttpClient httpClient, CancellationToken cancellationToken) =>
        _sessionProvider.GetAccessTokenIfNotExpiredAsync(cancellationToken)
            .Map(option => option
                .Map(token => new AuthenticationHeaderValue("Bearer", token))
                .Map(authHeader => httpClient.DefaultRequestHeaders.Authorization = authHeader))
            .Map(_ => httpClient);
}