using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Maui.Services.Infrastructure;
using SmartGrowHub.Shared.GrowHubs;
using SmartGrowHub.Shared.Results;

namespace SmartGrowHub.Maui.Services.Api;

public interface IGrowHubService
{
    IO<IEnumerable<GrowHubDto>> GetGrowHubs(CancellationToken cancellationToken);
}

public sealed class GrowHubService(HttpService httpService) : IGrowHubService
{
    public IO<IEnumerable<GrowHubDto>> GetGrowHubs(CancellationToken cancellationToken) =>
        httpService.GetAsync<Result<IEnumerable<GrowHubDto>>>(
            "/api/growHubs", cancellationToken
        ).Bind(result => result.ToOptionTIO()).ToFailIO(Error.New("Failed to get grow hubs"));
}