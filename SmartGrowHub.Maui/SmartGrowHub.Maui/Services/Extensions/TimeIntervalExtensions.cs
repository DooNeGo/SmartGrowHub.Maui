using SmartGrowHub.Shared.GrowHubs;

namespace SmartGrowHub.Maui.Services.Extensions;

public static class TimeIntervalExtensions
{
    public static bool IsOverlaps<TTime>(this TimeIntervalDto<TTime> interval, TimeIntervalDto<TTime> other)
        where TTime : IComparable<TTime> =>
        interval.Contains(other.Start) || interval.Contains(other.End) ||
        other.Contains(interval.Start) || other.Contains(interval.End);

    public static bool Contains<TTime>(this TimeIntervalDto<TTime> interval, TTime time)
        where TTime : IComparable<TTime> =>
        interval.IsCrossover() ? interval.IsWithinWrappedInterval(time) : interval.IsWithinNormalInterval(time);

    public static bool IsCrossover<TTime>(this TimeIntervalDto<TTime> interval) where TTime : IComparable<TTime> =>
        interval.Start.CompareTo(interval.End) > 0;
    
    private static bool IsWithinNormalInterval<TTime>(this TimeIntervalDto<TTime> interval, TTime time)
        where TTime : IComparable<TTime> =>
        time.CompareTo(interval.Start) > 0 && time.CompareTo(interval.End) < 0;

    private static bool IsWithinWrappedInterval<TTime>(this TimeIntervalDto<TTime> interval, TTime time)
        where TTime : IComparable<TTime> =>
        time.CompareTo(interval.Start) > 0 || time.CompareTo(interval.End) < 0;
}