using System.Net.Http.Headers;
using SmartGrowHub.Maui.Services.Extensions;

namespace SmartGrowHub.Maui.Services.DelegatingHandlers;

internal sealed class TokenDelegatingHandler : DelegatingHandler
{
    private readonly ISecureStorage _secureStorage;

    public TokenDelegatingHandler(ISecureStorage secureStorage) => _secureStorage = secureStorage;

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        string? accessToken = await _secureStorage.GetAccessTokenAsync().ConfigureAwait(false);
        
        if (!string.IsNullOrEmpty(accessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
        
        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}