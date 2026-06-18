using System.Collections.Immutable;
using SmartGrowHub.Maui.Services.Infrastructure;
using SmartGrowHub.Shared.GrowHubs.Model;
using SmartGrowHub.Shared.GrowHubs.Requests;
using SmartGrowHub.Shared.Results;

namespace SmartGrowHub.Maui.Services.Api;

public interface IGrowHubApi
{
    Task<ImmutableList<GrowHubDto>> GetGrowHubsAsync(CancellationToken cancellationToken);
    Task<ImmutableList<SensorMeasurementDto>> GetLatestMeasurementsAsync(string growHubId, CancellationToken cancellationToken);
    Task UpdateScheduleAsync(string scheduleId, UpdateScheduleRequestDto request, CancellationToken cancellationToken);
}

public sealed class GrowHubApi : IGrowHubApi
{
    private readonly IHttpService _httpService;

    public GrowHubApi(IHttpService httpService) => _httpService = httpService;

    public async Task<ImmutableList<GrowHubDto>> GetGrowHubsAsync(CancellationToken cancellationToken)
    {
        Result<IReadOnlyList<GrowHubDto>>? result = await _httpService
            .SendAsync<Result<IReadOnlyList<GrowHubDto>>>(
                HttpMethod.Get, new Uri("/api/grow-hubs", UriKind.Relative), null, cancellationToken)
            .ConfigureAwait(false);

        if (result is null || !result.IsSuccess || result.Data is null)
            throw new InvalidOperationException("Response was null or unsuccessful");

        return result.Data.ToImmutableList();
    }

    public async Task<ImmutableList<SensorMeasurementDto>> GetLatestMeasurementsAsync(string growHubId,
        CancellationToken cancellationToken)
    {
        Result<IReadOnlyList<SensorMeasurementDto>>? result = await _httpService
            .SendAsync<Result<IReadOnlyList<SensorMeasurementDto>>>(
                HttpMethod.Get, new Uri($"/api/grow-hubs/{growHubId}/sensors/measurements/latest", UriKind.Relative),
                null, cancellationToken)
            .ConfigureAwait(false);

        if (result is null || !result.IsSuccess || result.Data is null)
            throw new InvalidOperationException("Response was null or unsuccessful");

        return result.Data.ToImmutableList();
    }

    public async Task UpdateScheduleAsync(string scheduleId, UpdateScheduleRequestDto request,
        CancellationToken cancellationToken)
    {
        Result? result = await _httpService
            .SendAsJsonAsync<Result, UpdateScheduleRequestDto>(
                HttpMethod.Put, new Uri($"/api/grow-hubs/modules/schedules/{scheduleId}", UriKind.Relative), request,
                cancellationToken)
            .ConfigureAwait(false);

        if (result is null || !result.IsSuccess)
            throw new InvalidOperationException("Response was null or unsuccessful");
    }
}