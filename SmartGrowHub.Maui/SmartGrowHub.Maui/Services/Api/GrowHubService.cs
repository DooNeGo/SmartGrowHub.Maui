using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Maui.Services.Infrastructure;
using SmartGrowHub.Shared.GrowHubs;
using SmartGrowHub.Shared.Results;

namespace SmartGrowHub.Maui.Services.Api;

public interface IGrowHubService
{
    Eff<IEnumerable<GrowHubDto>> GetGrowHubs(CancellationToken cancellationToken);
}

public sealed class GrowHubService(HttpService httpService) : IGrowHubService
{
    public Eff<IEnumerable<GrowHubDto>> GetGrowHubs(CancellationToken cancellationToken) =>
        httpService.GetAsync<Result<IEnumerable<GrowHubDto>>>(
            "/api/growHubs", cancellationToken
        ).Run().As().Bind(option => option.Match(
            Some: result => result.ToEff(),
            None: () => Pure(Enumerable.Empty<GrowHubDto>())));
}