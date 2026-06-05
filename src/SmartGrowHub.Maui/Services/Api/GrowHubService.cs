using System.Collections.Immutable;
using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Maui.Services.Infrastructure;
using SmartGrowHub.Shared.GrowHubs.Model;
using SmartGrowHub.Shared.Results;

namespace SmartGrowHub.Maui.Services.Api;

public interface IGrowHubService
{
    IO<ImmutableList<GrowHubDto>> GetGrowHubs();
    IO<ImmutableList<SensorMeasurementDto>> GetLatestMeasurements(string growHubId);
}

public sealed class GrowHubService : IGrowHubService
{
    private readonly IHttpService _httpService;

    public GrowHubService(IHttpService httpService) => _httpService = httpService;

    public IO<ImmutableList<GrowHubDto>> GetGrowHubs() =>
        _httpService
            .GetAsync<Result<IEnumerable<GrowHubDto>>>("/api/grow-hubs")
            .ToIOOrFail("Response was null")
            .Bind(result => result.ToIO())
            .Map(x => x.ToImmutableList());

    public IO<ImmutableList<SensorMeasurementDto>> GetLatestMeasurements(string growHubId) =>
        _httpService
            .GetAsync<Result<IEnumerable<SensorMeasurementDto>>>($"/api/grow-hubs/{growHubId}/measurements/latest")
            .ToIOOrFail("Response was null")
            .Bind(result => result.ToIO())
            .Map(x => x.ToImmutableList());
}