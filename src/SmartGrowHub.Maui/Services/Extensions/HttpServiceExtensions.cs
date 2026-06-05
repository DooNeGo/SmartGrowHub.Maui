using System.Diagnostics.CodeAnalysis;
using SmartGrowHub.Maui.Services.Infrastructure;

namespace SmartGrowHub.Maui.Services.Extensions;

public static class HttpServiceExtensions
{
    private const UriKind DefaultUriKind = UriKind.RelativeOrAbsolute;

    extension(IHttpService httpService)
    {
        public OptionT<IO, TResponse> GetAsync<TResponse>([StringSyntax(StringSyntaxAttribute.Uri)] string uri) =>
            httpService.SendAsync<TResponse>(HttpMethod.Get, uri, null);

        public OptionT<IO, TResponse> PutAsync<TResponse>([StringSyntax(StringSyntaxAttribute.Uri)]
            string uri, HttpContent? content) =>
            httpService.SendAsync<TResponse>(HttpMethod.Put, uri, content);

        public OptionT<IO, TResponse> PostAsync<TResponse>([StringSyntax(StringSyntaxAttribute.Uri)]
            string uri, HttpContent? content) =>
            httpService.SendAsync<TResponse>(HttpMethod.Post, uri, content);

        public OptionT<IO, TResponse> DeleteAsync<TResponse>([StringSyntax(StringSyntaxAttribute.Uri)] string uri) =>
            httpService.SendAsync<TResponse>(HttpMethod.Delete, uri, null);

        public OptionT<IO, TResponse> PostAsJsonAsync<TResponse, TRequest>([StringSyntax(StringSyntaxAttribute.Uri)]
            string uri, TRequest request) =>
            httpService.SendJsonAsync<TResponse, TRequest>(HttpMethod.Post, uri, request);

        public OptionT<IO, TResponse> PutAsJsonAsync<TResponse, TRequest>([StringSyntax(StringSyntaxAttribute.Uri)]
            string uri, TRequest request) =>
            httpService.SendJsonAsync<TResponse, TRequest>(HttpMethod.Put, uri, request);

        public OptionT<IO, TResponse> SendJsonAsync<TResponse, TRequest>(HttpMethod method,
            [StringSyntax(StringSyntaxAttribute.Uri)]
            string uri, TRequest request) =>
            httpService.SendJsonAsync<TResponse, TRequest>(method, new Uri(uri, DefaultUriKind), request);

        public OptionT<IO, TResponse> SendAsync<TResponse>(HttpMethod method,
            [StringSyntax(StringSyntaxAttribute.Uri)]
            string uri, HttpContent? content) =>
            httpService.SendAsync<TResponse>(method, new Uri(uri, DefaultUriKind), content);
    }
}