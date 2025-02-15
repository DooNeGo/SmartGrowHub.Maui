using System.Net;
using SmartGrowHub.Maui.Services.Api;
using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Maui.Services.Flow;
using SmartGrowHub.Shared.Tokens;

namespace SmartGrowHub.Maui.Services.DelegatingHandlers;

internal sealed class UnauthorizedDelegatingHandler(
    IAuthService authService,
    ILogoutService logoutService,
    ISecureStorage secureStorage)
    : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken) => (
        from response in SendRequest(request, cancellationToken)
        from handledResponse in response.StatusCode is HttpStatusCode.Unauthorized
            ? HandleUnauthorizedResponse(request, cancellationToken)
            : Pure(response)
        select handledResponse
    ).RunAsync().AsTask();
    
    private IO<HttpResponseMessage> HandleUnauthorizedResponse(HttpRequestMessage request,
        CancellationToken cancellationToken) =>
        from _ in RefreshTokens(cancellationToken)
        from newResponse in SendRequest(request, cancellationToken)
        select newResponse;

    private IO<Unit> RefreshTokens(CancellationToken cancellationToken) => (
        from refreshToken in secureStorage.GetRefreshToken()
        from authTokens in RefreshTokens(refreshToken, cancellationToken).ToIO()
        from _ in secureStorage.SetAuthTokens(authTokens)
        select _
    ).IfNone(unit).As().ToEff().IfFail(_ => unit).RunIO();

    private OptionT<Eff, AuthTokensDto> RefreshTokens(string refreshToken, CancellationToken cancellationToken) =>
        OptionT<Eff, AuthTokensDto>.Lift(authService.RefreshTokens(refreshToken, cancellationToken).Run().As()
            | @catch(error => error.Code is 3, error =>
                logoutService.LogOut(cancellationToken).Bind(_ => FailEff<Option<AuthTokensDto>>(error))));
    
    private IO<HttpResponseMessage> SendRequest(HttpRequestMessage request, CancellationToken cancellationToken) =>
        IO.liftAsync(() => base.SendAsync(request, cancellationToken));
}