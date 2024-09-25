using SmartGrowHub.Domain.Features.RefreshTokens;
using SmartGrowHub.Maui.Services.Abstractions;
using System.Net.Http.Headers;

namespace SmartGrowHub.Maui.Services.Api;

public sealed class TokenDelegatingHandler(
    IUserSessionProvider sessionProvider,
    IAuthService authService)
    : DelegatingHandler
{
    private static bool s_isRefreshing;

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken) =>
        s_isRefreshing
            ? base.SendAsync(request, cancellationToken)
            : sessionProvider.GetAccessTokenIfNotExpiredAsync(cancellationToken)
                .Bind(option => option.Match(
                    Some: token => Pure(AddAccessToken(request, token)),
                    None: () => sessionProvider
                        .GetRefreshTokenAsync(cancellationToken)
                        .Bind(TurnOnRefreshing)
                        .Bind(option => option.Match(
                            Some: token =>
                            authService
                                .RefreshTokensAsync(new RefreshTokensRequest(token), cancellationToken)
                                .Map(response => AddAccessToken(request, response.AuthTokens.AccessToken)),
                            None: () =>
                            authService
                                .LogOutAsync(cancellationToken)
                                .MapFail(_ => Errors.Cancelled)
                                .Map(_ => request)))
                        .Bind(TurnOffRefreshing)))
                .Bind(_ => liftEff(() => base.SendAsync(request, cancellationToken)))
                .RunUnsafeAsync()
                .AsTask();

    private static HttpRequestMessage AddAccessToken(HttpRequestMessage request, AccessToken token)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return request;
    }

    private static Eff<T> TurnOnRefreshing<T>(T value) =>
        liftEff(() =>
        {
            s_isRefreshing = true;
            return value;
        });

    private static Eff<T> TurnOffRefreshing<T>(T value) =>
        liftEff(() =>
        {
            s_isRefreshing = false;
            return value;
        });
}
