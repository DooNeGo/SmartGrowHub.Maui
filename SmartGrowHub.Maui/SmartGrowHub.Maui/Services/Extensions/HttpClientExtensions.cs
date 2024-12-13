using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Json;

namespace SmartGrowHub.Maui.Services.Extensions;

public sealed class ResponseNullException : Exception;

public static class HttpClientExtensions
{
    public static Eff<TResponse> GetAsync<TResponse>(this HttpClient client,
        [StringSyntax(StringSyntaxAttribute.Uri)] string uri, CancellationToken cancellationToken) =>
        ExecuteAndParseResponseAsync<TResponse>(() => client.GetAsync(uri, cancellationToken), cancellationToken);

    public static Eff<TResponse> PutJsonAsync<TResponse, TValue>(this HttpClient client,
        [StringSyntax(StringSyntaxAttribute.Uri)] string uri, TValue request, CancellationToken cancellationToken) =>
        ExecuteAndParseResponseAsync<TResponse>(() =>
            client.PutAsJsonAsync(uri, request, cancellationToken), cancellationToken);

    public static Eff<TResponse> PutAsync<TResponse>(this HttpClient client,
        [StringSyntax(StringSyntaxAttribute.Uri)] string uri, HttpContent? content,
        CancellationToken cancellationToken) =>
        ExecuteAndParseResponseAsync<TResponse>(() =>
            client.PutAsync(uri, content, cancellationToken), cancellationToken);

    public static Eff<TResponse> PostJsonAsync<TResponse, TRequest>(this HttpClient client,
        [StringSyntax(StringSyntaxAttribute.Uri)] string uri, TRequest request,
        CancellationToken cancellationToken) =>
        ExecuteAndParseResponseAsync<TResponse>(() =>
            client.PostAsJsonAsync(uri, request, cancellationToken), cancellationToken);

    public static Eff<TResponse> PostAsync<TResponse>(this HttpClient client,
        [StringSyntax(StringSyntaxAttribute.Uri)] string uri, HttpContent? content,
        CancellationToken cancellationToken) =>
        ExecuteAndParseResponseAsync<TResponse>(() =>
            client.PostAsync(uri, content, cancellationToken), cancellationToken);

    public static Eff<TResponse> DeleteAsync<TResponse>(this HttpClient client,
        [StringSyntax(StringSyntaxAttribute.Uri)] string uri, CancellationToken cancellationToken) =>
        ExecuteAndParseResponseAsync<TResponse>(() =>
            client.DeleteAsync(uri, cancellationToken), cancellationToken);

    private static Eff<TResponse> ExecuteAndParseResponseAsync<TResponse>(
        Func<Task<HttpResponseMessage>> func, CancellationToken cancellationToken) =>
        liftEff(async () =>
        {
            using HttpResponseMessage response = await func().ConfigureAwait(false);

            return await response.Content
                       .ReadFromJsonAsync<TResponse>(cancellationToken)
                       .ConfigureAwait(false)
                   ?? throw new ResponseNullException();
        });
}