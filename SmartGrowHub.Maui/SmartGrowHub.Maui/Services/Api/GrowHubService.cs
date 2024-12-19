using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Shared.GrowHubs;
using SmartGrowHub.Shared.Results;

namespace SmartGrowHub.Maui.Services.Api;

public interface IGrowHubService
{
    Eff<IEnumerable<GrowHubDto>> GetGrowHubs(CancellationToken cancellationToken);
}

public sealed class GrowHubService(HttpClient httpClient) : IGrowHubService
{
    public Eff<IEnumerable<GrowHubDto>> GetGrowHubs(CancellationToken cancellationToken) =>
        httpClient
            .GetAsync<Result<IEnumerable<GrowHubDto>>>("/api/growHubs", cancellationToken)
            .Bind(result => result.ToEff());
}