using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;

namespace SmartGrowHub.Maui.Services.Infrastructure;


public sealed class HttpService(IHttpClientFactory clientFactory, IJsonSerializerService jsonSerializer)
{
    public string ClientName { get; set; } = string.Empty;

    private HttpClient HttpClient => string.IsNullOrEmpty(ClientName)
        ? clientFactory.CreateClient()
        : clientFactory.CreateClient(ClientName);
    
    public OptionT<Eff, TResponse> GetAsync<TResponse>([StringSyntax(StringSyntaxAttribute.Uri)] string uri,
        CancellationToken cancellationToken) =>
        ExecuteAndParseResponseAsync<TResponse>(() => HttpClient.GetAsync(uri, cancellationToken), cancellationToken);

    public OptionT<Eff, TResponse> PutJsonAsync<TResponse, TValue>([StringSyntax(StringSyntaxAttribute.Uri)] string uri,
        TValue request, CancellationToken cancellationToken) =>
        ExecuteAndParseResponseAsync<TResponse>(() =>
            HttpClient.PutAsJsonAsync(uri, request, cancellationToken), cancellationToken);

    public OptionT<Eff, TResponse> PutAsync<TResponse>([StringSyntax(StringSyntaxAttribute.Uri)] string uri,
        HttpContent? content, CancellationToken cancellationToken) =>
        ExecuteAndParseResponseAsync<TResponse>(() =>
            HttpClient.PutAsync(uri, content, cancellationToken), cancellationToken);

    public OptionT<Eff, TResponse> PostJsonAsync<TResponse, TRequest>(
        [StringSyntax(StringSyntaxAttribute.Uri)] string uri, TRequest request, CancellationToken cancellationToken) =>
        ExecuteAndParseResponseAsync<TResponse>(() =>
            HttpClient.PostAsJsonAsync(uri, request, cancellationToken), cancellationToken);

    public OptionT<Eff, TResponse> PostAsync<TResponse>([StringSyntax(StringSyntaxAttribute.Uri)] string uri,
        HttpContent? content, CancellationToken cancellationToken) =>
        ExecuteAndParseResponseAsync<TResponse>(() =>
            HttpClient.PostAsync(uri, content, cancellationToken), cancellationToken);

    public OptionT<Eff, TResponse> DeleteAsync<TResponse>([StringSyntax(StringSyntaxAttribute.Uri)] string uri,
        CancellationToken cancellationToken) =>
        ExecuteAndParseResponseAsync<TResponse>(() => HttpClient.DeleteAsync(uri, cancellationToken), cancellationToken);

    private OptionT<Eff, TResponse> ExecuteAndParseResponseAsync<TResponse>(Func<Task<HttpResponseMessage>> func,
        CancellationToken cancellationToken) =>
        from response in IO.liftAsync(func).Bracket()
        from stream in IO.liftAsync(() => response.Content.ReadAsStreamAsync(cancellationToken)).Bracket()
        from result in jsonSerializer.DeserializeAsync<TResponse>(stream, cancellationToken)
        select result;
}