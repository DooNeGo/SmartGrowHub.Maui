using System.Net;
using SmartGrowHub.Maui.Services.Flow;

namespace SmartGrowHub.Maui.Services.DelegatingHandlers;

internal sealed class AuthorizationErrorDelegatingHandler(
    IAuthorizationErrorHandler authorizationErrorHandler)
    : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken) =>
        liftEff(() => base.SendAsync(request, cancellationToken))
            .Bind(response => HandleResponse(response, cancellationToken))
            .RunUnsafeAsync().AsTask();

    private Eff<HttpResponseMessage> HandleResponse(
        HttpResponseMessage response, CancellationToken cancellationToken) =>
        response.StatusCode is HttpStatusCode.Unauthorized
            ? authorizationErrorHandler.Handle(cancellationToken).Map(_ => response)
            : Pure(response);
}
