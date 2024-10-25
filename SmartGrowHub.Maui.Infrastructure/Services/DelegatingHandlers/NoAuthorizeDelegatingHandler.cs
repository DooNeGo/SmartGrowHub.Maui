using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Maui.Application.Interfaces;
using System.Net;

namespace SmartGrowHub.Maui.Infrastructure.Services.DelegatingHandlers;

internal sealed class NoAuthorizeDelegatingHandler(INoAuthorizeService noAuthorizeService) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
        base.SendAsync(request, cancellationToken).ToEff()
            .Bind(response => HandleResponse(response, cancellationToken))
            .RunUnsafeAsync().AsTask();

    private Eff<HttpResponseMessage> HandleResponse(HttpResponseMessage response, CancellationToken cancellationToken) =>
        response.StatusCode is HttpStatusCode.Unauthorized
            ? noAuthorizeService.Handle(cancellationToken).Map(_ => response)
            : Pure(response);
}
