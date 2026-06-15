using System.Net.Http.Json;

namespace SmartGrowHub.Maui.Services.Infrastructure;

public interface IHttpService
{
    string ClientName { get; set; }

    OptionT<IO, T> SendAsJson<T, TRequest>(HttpMethod method, Uri? uri, TRequest request);
    OptionT<IO, T> Send<T>(HttpMethod method, Uri? uri, HttpContent? content);
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

    public OptionT<IO, T> SendAsJson<T, TRequest>(HttpMethod method, Uri? uri, TRequest request)
    {
        var content = JsonContent.Create(request);
        return Send<T>(method, uri, content);
    }

    public OptionT<IO, T> Send<T>(HttpMethod method, Uri? uri, HttpContent? content) =>
        OptionT.lift(IO.liftAsync(async env =>
        {
            HttpClient client = _clientFactory.CreateClient(ClientName);
            using var request = new HttpRequestMessage(method, uri);
            request.Content = content;

            using HttpResponseMessage response = await client
                .SendAsync(request, HttpCompletionOption.ResponseHeadersRead, env.Token)
                .ConfigureAwait(false);

            //string stringResponse = await response.Content.ReadAsStringAsync(env.Token);
            await using Stream stream = await response.Content.ReadAsStreamAsync(env.Token).ConfigureAwait(false);
            Option<T> result = await _jsonSerializer.DeserializeAsync<T>(stream, env.Token).ConfigureAwait(false);
            return result;
        }));
}