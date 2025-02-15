using SmartGrowHub.Shared.GrowHubs;

namespace SmartGrowHub.Maui.Services.Extensions;

public static class TimedQuantityExtensions
{
    public static Option<TimedQuantityDto<TTime>> FindByTime<TTime>(
        this IEnumerable<TimedQuantityDto<TTime>> timedQuantities, TTime time) where TTime : IComparable<TTime> =>
        Optional(timedQuantities.FirstOrDefault(timedQuantity => timedQuantity.Interval.Contains(time)));
}