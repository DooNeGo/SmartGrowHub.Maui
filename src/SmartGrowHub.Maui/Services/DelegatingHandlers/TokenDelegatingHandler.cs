using System.Net.Http.Headers;
using SmartGrowHub.Maui.Services.Extensions;

namespace SmartGrowHub.Maui.Services.DelegatingHandlers;

internal sealed class TokenDelegatingHandler(ISecureStorage secureStorage) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken) => (
        from _ in SetAuthorization(request)
        from response in SendRequest(request)
        select response
    ).RunAsync(cancellationToken).AsTask();

    private IO<HttpResponseMessage> SendRequest(HttpRequestMessage request) =>
        IO.liftAsync(env => base.SendAsync(request, env.Token));

    private IO<Unit> SetAuthorization(HttpRequestMessage request) => (
        from accessToken in secureStorage.GetAccessToken()
        from _ in request.SetAccessToken(accessToken)
        select _
    ).IfNone(Unit.Default).As();
}

public static class HttpRequestMessageExtension
{
    private const string BearerScheme = "Bearer";
    
    public static IO<Unit> SetAccessToken(this HttpRequestMessage request, string accessToken) =>
        IO.lift(() => request.Headers.Authorization = new AuthenticationHeaderValue(BearerScheme, accessToken))
            .ToUnit();
}
