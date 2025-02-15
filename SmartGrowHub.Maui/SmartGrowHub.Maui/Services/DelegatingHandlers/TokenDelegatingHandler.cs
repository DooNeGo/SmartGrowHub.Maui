using System.Net.Http.Headers;
using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Maui.Services.Flow;
using SmartGrowHub.Maui.Services.Infrastructure;
using Eff = LanguageExt.Eff;

namespace SmartGrowHub.Maui.Services.DelegatingHandlers;

internal sealed class TokenDelegatingHandler(ISecureStorage secureStorage) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken) => (
        from _ in SetAuthorization(request)
        from response in SendRequest(request, cancellationToken)
        select response
    ).RunAsync().AsTask();

    private IO<HttpResponseMessage> SendRequest(HttpRequestMessage request, CancellationToken cancellationToken) =>
        IO.liftAsync(() => base.SendAsync(request, cancellationToken));

    private IO<Unit> SetAuthorization(HttpRequestMessage request) => (
        from accessToken in secureStorage.GetAccessToken()
        from _ in request.SetAccessToken(accessToken)
        select _
    ).IfNone(unit).As();
}

public static class HttpRequestMessageExtension
{
    private const string BearerScheme = "Bearer";
    
    public static IO<Unit> SetAccessToken(this HttpRequestMessage request, string accessToken) =>
        IO.lift(() => request.Headers.Authorization = new AuthenticationHeaderValue(BearerScheme, accessToken))
            .Map(_ => unit);
}
