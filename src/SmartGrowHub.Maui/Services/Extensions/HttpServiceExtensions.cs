using SmartGrowHub.Maui.Services.Infrastructure;

namespace SmartGrowHub.Maui.Services.Extensions;

public static class HttpServiceExtensions
{
    private const UriKind DefaultUriKind = UriKind.RelativeOrAbsolute;

    extension(IHttpService httpService)
    {
        public OptionT<IO, TResponse> Get<TResponse>(string uri) =>
            httpService.Send<TResponse>(HttpMethod.Get, uri, null);

        public OptionT<IO, TResponse> Put<TResponse>(string uri, HttpContent? content) =>
            httpService.Send<TResponse>(HttpMethod.Put, uri, content);

        public OptionT<IO, TResponse> Post<TResponse>(string uri, HttpContent? content) =>
            httpService.Send<TResponse>(HttpMethod.Post, uri, content);

        public OptionT<IO, TResponse> Delete<TResponse>(string uri) =>
            httpService.Send<TResponse>(HttpMethod.Delete, uri, null);

        public OptionT<IO, TResponse> PostAsJson<TResponse, TRequest>(string uri, TRequest request) =>
            httpService.SendAsJson<TResponse, TRequest>(HttpMethod.Post, uri, request);

        public OptionT<IO, TResponse> PutAsJson<TResponse, TRequest>(string uri, TRequest request) =>
            httpService.SendAsJson<TResponse, TRequest>(HttpMethod.Put, uri, request);

        public OptionT<IO, TResponse> SendAsJson<TResponse, TRequest>(HttpMethod method, string uri, TRequest request) =>
            httpService.SendAsJson<TResponse, TRequest>(method, new Uri(uri, DefaultUriKind), request);

        public OptionT<IO, TResponse> Send<TResponse>(HttpMethod method, string uri, HttpContent? content) =>
            httpService.Send<TResponse>(method, new Uri(uri, DefaultUriKind), content);
    }
}