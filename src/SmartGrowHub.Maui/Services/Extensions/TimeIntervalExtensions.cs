using SmartGrowHub.Shared.GrowHubs.Model;

namespace SmartGrowHub.Maui.Services.Extensions;

public static class TimeIntervalExtensions
{
    extension<TTime>(TimeIntervalDto<TTime> interval) where TTime : IComparable<TTime>
    {
        public bool IsOverlaps(TimeIntervalDto<TTime> other) =>
            interval.Contains(other.Start) || interval.Contains(other.End) ||
            other.Contains(interval.Start) || other.Contains(interval.End);

        public bool Contains(TTime time) =>
            interval.IsCrossover ? interval.IsWithinWrappedInterval(time) : interval.IsWithinNormalInterval(time);

        public bool IsCrossover => interval.Start.CompareTo(interval.End) > 0;

        private bool IsWithinNormalInterval(TTime time) =>
            time.CompareTo(interval.Start) > 0 && time.CompareTo(interval.End) < 0;

        private bool IsWithinWrappedInterval(TTime time) =>
            time.CompareTo(interval.Start) > 0 || time.CompareTo(interval.End) < 0;
    }
}