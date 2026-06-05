using System.Text;
using SmartGrowHub.Maui.Services.Extensions;

namespace SmartGrowHub.Maui.Services.Infrastructure;

public interface IHttpService
{
    string ClientName { get; set; }

    OptionT<IO, T> SendJsonAsync<T, TRequest>(HttpMethod method, Uri? uri, TRequest request);
    OptionT<IO, T> SendAsync<T>(HttpMethod method, Uri? uri, HttpContent? content);
}

public sealed class HttpService(IHttpClientFactory clientFactory, IJsonSerializer jsonSerializer) : IHttpService
{
    private const string MediaType = "application/json";

    private HttpClient HttpClient => clientFactory.CreateClient(ClientName);

    public string ClientName { get; set; } = string.Empty;

    public OptionT<IO, T> SendJsonAsync<T, TRequest>(HttpMethod method, Uri? uri, TRequest request) =>
        from json in OptionT.lift(jsonSerializer.Serialize(request).ToIO())
        let content = new StringContent(json, Encoding.UTF8, MediaType)
        from response in SendAsync<T>(method, uri, content)
        select response;

    public OptionT<IO, T> SendAsync<T>(HttpMethod method, Uri? uri, HttpContent? content) =>
        OptionT.lift(IO.liftAsync(async env =>
        {
            using HttpRequestMessage request = CreateHttpRequest(method, uri, content);
            using HttpResponseMessage response = await HttpClient.SendAsync(request, env.Token).ConfigureAwait(false);
            await using Stream stream = await response.Content.ReadAsStreamAsync(env.Token).ConfigureAwait(false);
            return await jsonSerializer.DeserializeAsync<T>(stream, env.Token).ConfigureAwait(false);
        }).Map(Prelude.Optional));

    private static HttpRequestMessage CreateHttpRequest(HttpMethod method, Uri? uri, HttpContent? content)
    {
        var request = new HttpRequestMessage(method, uri);
        request.Content = content;
        return request;
    }
}