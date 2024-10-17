
namespace SmartGrowHub.Maui.Infrastructure.Services.DelegatingHandlers;

internal sealed class BadConnectionDelegatingHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        liftEff(() => base.SendAsync(request, cancellationToken));

        throw new NotImplementedException();
    }
}
