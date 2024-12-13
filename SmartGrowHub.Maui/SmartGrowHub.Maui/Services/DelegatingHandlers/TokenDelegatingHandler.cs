using System.Net.Http.Headers;
using SmartGrowHub.Maui.Services.Flow;
using SmartGrowHub.Maui.Services.Infrastructure;

namespace SmartGrowHub.Maui.Services.DelegatingHandlers;

internal sealed class TokenDelegatingHandler(
    ITokensStorage tokensStorage,
    IRefreshTokensService refreshTokensService)
    : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
        tokensStorage
            .GetAccessTokenIfNotExpired(cancellationToken)
            .IfFailEff(_ => GetAndRefreshTokens(cancellationToken))
            .Bind(accessToken => SetAuthorization(request, accessToken))
            .Bind(_ => liftEff(() => base.SendAsync(request, cancellationToken)))
            .RunUnsafeAsync().AsTask();

    private static IO<Unit> SetAuthorization(HttpRequestMessage request, string accessToken) =>
        lift(() =>
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        });

    private Eff<string> GetAndRefreshTokens(CancellationToken cancellationToken) =>
        from tokens in refreshTokensService.RefreshTokens(cancellationToken)
        select tokens.AccessToken;
}
