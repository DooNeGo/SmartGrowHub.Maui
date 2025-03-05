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
            .ToFailIO(Error.Empty)
            .TapOnFail(_ => logoutService.LogOut(cancellationToken))
        from authTokens in RefreshTokens(refreshToken, cancellationToken)
        from _ in secureStorage.SetAuthTokens(authTokens)
        select _
    ).ToFailIO(Error.New("Failed to refresh tokens"));

    private OptionT<IO, AuthTokensDto> RefreshTokens(string refreshToken, CancellationToken cancellationToken) =>
        authService.RefreshTokens(refreshToken, cancellationToken).Run().As()
            .TapOnFail(error => error.Code is 3
                ? logoutService.LogOut(cancellationToken)
                : IO.pure(Unit.Default))
            .Retry(Schedule.Once);
    
    private IO<HttpResponseMessage> SendRequest(HttpRequestMessage request, CancellationToken cancellationToken) =>
        IO.liftAsync(() => base.SendAsync(request, cancellationToken));
}