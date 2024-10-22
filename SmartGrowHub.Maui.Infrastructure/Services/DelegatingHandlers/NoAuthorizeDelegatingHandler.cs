using Mediator;
using SmartGrowHub.Domain.Extensions;
using SmartGrowHub.Maui.Application.Notifications;
using System.Net;

namespace SmartGrowHub.Maui.Infrastructure.Services.DelegatingHandlers;

internal sealed class NoAuthorizeDelegatingHandler(IMediator mediator) : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) =>
        base.SendAsync(request, cancellationToken).ToIO()
            .Bind(response => HandleResponse(response, cancellationToken))
            .RunAsync().AsTask();

    private IO<HttpResponseMessage> HandleResponse(HttpResponseMessage response, CancellationToken cancellationToken) =>
        response.StatusCode is HttpStatusCode.Unauthorized
            ? SendNotification(cancellationToken).Map(_ => response)
            : Pure(response);

    private IO<Unit> SendNotification(CancellationToken cancellationToken) =>
        mediator.Publish(NoAuthorizeNotification.Default, cancellationToken).ToIO();
}
