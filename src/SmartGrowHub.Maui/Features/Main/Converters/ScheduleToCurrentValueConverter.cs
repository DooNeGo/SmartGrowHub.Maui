using System.Globalization;
using SmartGrowHub.Maui.Resources.Localization;
using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Shared.GrowHubs.Model;

namespace SmartGrowHub.Maui.Features.Main.Converters;

public sealed class ScheduleToCurrentValueConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is ScheduleDto schedule
            ? schedule switch
            {
                DisabledScheduleDto d => MapDisabledToString(d),
                EnabledScheduleDto e => MapEnabledToString(e),
                DailyScheduleDto d => MapDailyToString(d),
                WeeklyScheduleDto w => MapWeeklyToString(w),
                _ => value
            }
            : value;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }

    private static string MapDisabledToString(DisabledScheduleDto disabledSchedule) => AppResources.TurnedOffShort;
    
    private static string MapEnabledToString(EnabledScheduleDto enabledSchedule) => "On";
    
    private static string MapDailyToString(DailyScheduleDto daily) =>
        GetFormattedCurrentValue(daily.Entries, TimeOnly.FromDateTime(DateTime.Now));

    private static string MapWeeklyToString(WeeklyScheduleDto weekly)
    {
        DateTime dateTimeNow = DateTime.Now;
        TimeOnly timeNow = TimeOnly.FromDateTime(dateTimeNow);
        WeekTimeOnlyDto weekTime = new(dateTimeNow.DayOfWeek, timeNow);
        
        return GetFormattedCurrentValue(weekly.Entries, weekTime);
    }

    private static string GetFormattedCurrentValue<TTime>(
        IReadOnlyList<ScheduleUnitDto<TTime>> units, in TTime timeNow) where TTime : IComparable<TTime>
    {
        var unit = units.FindByTime(timeNow);
        return unit is not null ? QuantityToString(unit.Quantity) : AppResources.TurnedOffShort;
    }

    private static string QuantityToString(QuantityDto quantity) => $"{quantity.Magnitude} {quantity.Unit}";
}