using System.Net;
using Serilog;
using SmartGrowHub.Maui.Services.Api;
using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Maui.Services.Flow;
using SmartGrowHub.Shared.Tokens;

namespace SmartGrowHub.Maui.Services.DelegatingHandlers;

internal sealed class UnauthorizedDelegatingHandler : DelegatingHandler
{
    private readonly IAuthService _authService;
    private readonly ILogoutService _logoutService;
    private readonly ISecureStorage _secureStorage;

    public UnauthorizedDelegatingHandler(
        IAuthService authService,
        ILogoutService logoutService,
        ISecureStorage secureStorage)
    {
        _authService = authService;
        _logoutService = logoutService;
        _secureStorage = secureStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);

        if (response.StatusCode is HttpStatusCode.Unauthorized)
        {
            response = await HandleUnauthorizedResponse(request).RunAsync(cancellationToken).ConfigureAwait(false);
        }

        return response;

    // return (
    //         from response1 in SendRequest(request)
    //         from handledResponse in response.StatusCode is HttpStatusCode.Unauthorized
    //             ? HandleUnauthorizedResponse(request)
    //             : IO.pure(response)
    //         select handledResponse
    //     ).RunAsync(EnvIO.New(token: cancellationToken)).AsTask();
    }

    private IO<HttpResponseMessage> HandleUnauthorizedResponse(HttpRequestMessage request) =>
        from _ in RefreshTokens()
        from newResponse in SendRequest(request)
        select newResponse;

    private IO<Unit> RefreshTokens() =>
        from refreshToken in _secureStorage.GetRefreshToken()
            .ToIOOrFail("No refresh token found")
            .TapOnFail(_ => _logoutService.LogOut())
        from authTokens in RefreshTokens(refreshToken)
        from _ in _secureStorage.SetAuthTokens(authTokens)
        select _;

    private IO<AuthTokensDto> RefreshTokens(string refreshToken) =>
        _authService.RefreshTokens(refreshToken)
            .Retry(Schedule.Once)
            .TapOnFail(error => error.Code is 3
                ? _logoutService.LogOut()
                : IO.pure(Unit.Default));
    
    private IO<HttpResponseMessage> SendRequest(HttpRequestMessage request) =>
        IO.liftAsync(env => base.SendAsync(request, env.Token));
}