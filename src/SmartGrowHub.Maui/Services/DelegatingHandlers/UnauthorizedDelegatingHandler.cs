using System.Net;
using SmartGrowHub.Maui.Services.App;

namespace SmartGrowHub.Maui.Services.DelegatingHandlers;

internal sealed class UnauthorizedDelegatingHandler : DelegatingHandler
{
    private readonly IAuthService _authService;

    public UnauthorizedDelegatingHandler(IAuthService authService) => _authService = authService;

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
        from _ in _authService.RefreshTokens()
        from newResponse in SendRequest(request)
        select newResponse;
    
    private IO<HttpResponseMessage> SendRequest(HttpRequestMessage request) =>
        IO.liftAsync(env => base.SendAsync(request, env.Token));
}