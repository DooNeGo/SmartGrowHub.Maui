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
            await _authService.RefreshTokens(cancellationToken).ConfigureAwait(false);
            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }
        
        return response;
    }
}