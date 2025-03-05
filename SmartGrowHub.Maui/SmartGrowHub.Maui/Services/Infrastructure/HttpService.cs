using System.Text;

namespace SmartGrowHub.Maui.Services.Infrastructure;

public interface IHttpService
{
    string ClientName { get; set; }

    OptionT<IO, T> SendJsonAsync<T, TRequest>(HttpMethod method, Uri? uri, TRequest request,
        CancellationToken cancellationToken);
    
    OptionT<IO, T> SendAsync<T>(HttpMethod method, Uri? uri, HttpContent? content,
        CancellationToken cancellationToken);
}

public sealed class HttpService(IHttpClientFactory clientFactory, IJsonSerializer jsonSerializer) : IHttpService
{
    private const string MediaType = "application/json";

    private HttpClient HttpClient => clientFactory.CreateClient(ClientName);

    public string ClientName { get; set; } = string.Empty;

    public OptionT<IO, T> SendJsonAsync<T, TRequest>(HttpMethod method, Uri? uri, TRequest request,
        CancellationToken cancellationToken) =>
        from json in IO.pure(jsonSerializer.Serialize(request))
        let content = new StringContent(json, Encoding.UTF8, MediaType)
        from response in SendAsync<T>(method, uri, content, cancellationToken)
        select response;

    public OptionT<IO, T> SendAsync<T>(HttpMethod method, Uri? uri, HttpContent? content,
        CancellationToken cancellationToken) => (
        from request in IO.pure(CreateHttpRequest(method, uri, content))
        from response in IO.liftAsync(() => HttpClient.SendAsync(request, cancellationToken))
        from stream in IO.liftAsync(() => response.Content.ReadAsStreamAsync(cancellationToken))
        from result in jsonSerializer.DeserializeAsync<T>(stream, cancellationToken)
        select result
    ).Run().As().Bracket();

    private static HttpRequestMessage CreateHttpRequest(HttpMethod method, Uri? uri, HttpContent? content)
    {
        var request = new HttpRequestMessage(method, uri);
        request.Content = content;
        return request;
    }
}