using System.Net.Http.Json;

namespace SmartGrowHub.Maui.Services.Infrastructure;

public interface IHttpService
{
    string ClientName { get; set; }

    Task<T?> SendAsJsonAsync<T, TRequest>(HttpMethod method, Uri? uri, TRequest request, CancellationToken cancellationToken);
    Task<T?> SendAsync<T>(HttpMethod method, Uri? uri, HttpContent? content, CancellationToken cancellationToken);
}

public sealed class HttpService : IHttpService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly IJsonSerializer _jsonSerializer;

    public HttpService(IHttpClientFactory clientFactory, IJsonSerializer jsonSerializer)
    {
        _clientFactory = clientFactory;
        _jsonSerializer = jsonSerializer;
    }

    public string ClientName { get; set; } = string.Empty;

    public Task<T?> SendAsJsonAsync<T, TRequest>(HttpMethod method, Uri? uri, TRequest request, CancellationToken cancellationToken)
    {
        var content = JsonContent.Create(request);
        return SendAsync<T>(method, uri, content, cancellationToken);
    }

    public async Task<T?> SendAsync<T>(HttpMethod method, Uri? uri, HttpContent? content, CancellationToken cancellationToken)
    {
        HttpClient client = _clientFactory.CreateClient(ClientName);
        using var request = new HttpRequestMessage(method, uri);
        request.Content = content;

        using HttpResponseMessage response = await client
            .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
            .ConfigureAwait(false);

        await using Stream stream = await response.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        T? result = await _jsonSerializer.DeserializeAsync<T>(stream, cancellationToken).ConfigureAwait(false);
        return result;
    }
}