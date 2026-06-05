
namespace SmartGrowHub.Maui.Services.DelegatingHandlers;

internal sealed class BadConnectionDelegatingHandler : DelegatingHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
