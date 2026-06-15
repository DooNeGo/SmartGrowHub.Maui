using System.Collections.Immutable;
using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Maui.Services.Infrastructure;
using SmartGrowHub.Shared.GrowHubs.Model;
using SmartGrowHub.Shared.GrowHubs.Requests;
using SmartGrowHub.Shared.Results;

namespace SmartGrowHub.Maui.Services.Api;

public interface IGrowHubApi
{
    IO<ImmutableList<GrowHubDto>> GetGrowHubs();
    IO<ImmutableList<SensorMeasurementDto>> GetLatestMeasurements(string growHubId);
    IO<Unit> UpdateSchedule(string scheduleId, UpdateScheduleRequestDto request);
}

public sealed class GrowHubApi : IGrowHubApi
{
    private readonly IHttpService _httpService;

    public GrowHubApi(IHttpService httpService) => _httpService = httpService;

    public IO<ImmutableList<GrowHubDto>> GetGrowHubs() =>
        _httpService
            .Get<Result<IReadOnlyList<GrowHubDto>>>("/api/grow-hubs")
            .ToIOOrFail("Response was null")
            .Bind(result => result.ToIO())
            .Map(x => x.ToImmutableList());

    public IO<ImmutableList<SensorMeasurementDto>> GetLatestMeasurements(string growHubId) =>
        _httpService
            .Get<Result<IReadOnlyList<SensorMeasurementDto>>>(
                $"/api/grow-hubs/{growHubId}/sensors/measurements/latest")
            .ToIOOrFail("Response was null")
            .Bind(result => result.ToIO())
            .Map(x => x.ToImmutableList());

    public IO<Unit> UpdateSchedule(string scheduleId, UpdateScheduleRequestDto request) =>
        _httpService
            .PutAsJson<Result, UpdateScheduleRequestDto>(
                $"/api/grow-hubs/modules/schedules/{scheduleId}", request)
            .ToIOOrFail("Response was null")
            .Bind(result => result.ToIO());
}