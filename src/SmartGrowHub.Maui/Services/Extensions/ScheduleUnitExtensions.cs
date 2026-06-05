using SmartGrowHub.Shared.GrowHubs.Model;

namespace SmartGrowHub.Maui.Services.Extensions;

public static class ScheduleUnitExtensions
{
    public static Option<ScheduleUnitDto<TTime>> FindByTime<TTime>(
        this IEnumerable<ScheduleUnitDto<TTime>> units, TTime time) where TTime : IComparable<TTime> =>
        units.FirstOrDefault(unit => unit.Interval.Contains(time));
}