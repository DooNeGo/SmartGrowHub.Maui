using System.Net.Http.Headers;

namespace SmartGrowHub.Maui.Services.Api;

public sealed class TokenDelegatingHandler(ITokenStorage tokenProvider) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken) =>
        tokenProvider.GetAsync(cancellationToken)
            .Map(token => new AuthenticationHeaderValue("Bearer", token))
            .IfSome(authentication => request.Headers.Authorization = authentication)
            .MapAsync(_ => base.SendAsync(request, cancellationToken));
}
