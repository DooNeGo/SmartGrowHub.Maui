using System.Diagnostics.CodeAnalysis;
using SmartGrowHub.Maui.Services.Infrastructure;

namespace SmartGrowHub.Maui.Services.Extensions;

public static class HttpServiceExtensions
{
    private const UriKind DefaultUriKind = UriKind.RelativeOrAbsolute;

    public static OptionT<IO, TResponse> GetAsync<TResponse>(this IHttpService httpService,
        [StringSyntax(StringSyntaxAttribute.Uri)]
        string uri, CancellationToken cancellationToken) =>
        httpService.SendAsync<TResponse>(HttpMethod.Get, uri, null, cancellationToken);

    public static OptionT<IO, TResponse> PutAsync<TResponse>(this IHttpService httpService,
        [StringSyntax(StringSyntaxAttribute.Uri)]
        string uri, HttpContent? content, CancellationToken cancellationToken) =>
        httpService.SendAsync<TResponse>(HttpMethod.Put, uri, content, cancellationToken);

    public static OptionT<IO, TResponse> PostAsync<TResponse>(this IHttpService httpService,
        [StringSyntax(StringSyntaxAttribute.Uri)]
        string uri, HttpContent? content, CancellationToken cancellationToken) =>
        httpService.SendAsync<TResponse>(HttpMethod.Post, uri, content, cancellationToken);

    public static OptionT<IO, TResponse> DeleteAsync<TResponse>(this IHttpService httpService,
        [StringSyntax(StringSyntaxAttribute.Uri)]
        string uri, CancellationToken cancellationToken) =>
        httpService.SendAsync<TResponse>(HttpMethod.Delete, uri, null, cancellationToken);

    public static OptionT<IO, TResponse> PostAsJsonAsync<TResponse, TRequest>(this IHttpService httpService,
        [StringSyntax(StringSyntaxAttribute.Uri)]
        string uri, TRequest request, CancellationToken cancellationToken) =>
        httpService.SendJsonAsync<TResponse, TRequest>(HttpMethod.Post, uri, request, cancellationToken);

    public static OptionT<IO, TResponse> PutAsJsonAsync<TResponse, TRequest>(this IHttpService httpService,
        [StringSyntax(StringSyntaxAttribute.Uri)]
        string uri, TRequest request, CancellationToken cancellationToken) =>
        httpService.SendJsonAsync<TResponse, TRequest>(HttpMethod.Put, uri, request, cancellationToken);

    public static OptionT<IO, TResponse> SendJsonAsync<TResponse, TRequest>(this IHttpService httpService, HttpMethod method,
        [StringSyntax(StringSyntaxAttribute.Uri)]
        string uri, TRequest request, CancellationToken cancellationToken) =>
        httpService.SendJsonAsync<TResponse, TRequest>(method, new Uri(uri, DefaultUriKind), request,
            cancellationToken);

    public static OptionT<IO, TResponse> SendAsync<TResponse>(this IHttpService httpService, HttpMethod method,
        [StringSyntax(StringSyntaxAttribute.Uri)]
        string uri, HttpContent? content, CancellationToken cancellationToken) =>
        httpService.SendAsync<TResponse>(method, new Uri(uri, DefaultUriKind), content, cancellationToken);
}