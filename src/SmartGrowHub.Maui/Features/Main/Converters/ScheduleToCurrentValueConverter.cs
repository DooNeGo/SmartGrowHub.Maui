using System.Globalization;
using SmartGrowHub.Maui.Resources.Localization;
using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Shared.GrowHubs.Model;

namespace SmartGrowHub.Maui.Features.Main.Converters;

public sealed class ScheduleToCurrentValueConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is ScheduleDto schedule
            ? schedule.Match(MapDisabledToString, MapEnabledToString, MapDailyToString, MapWeeklyToString)
            : value;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => value;

    private static string MapDisabledToString(DisabledScheduleDto disabledSchedule) => AppResources.TurnedOffShort;
    
    private static string MapEnabledToString(EnabledScheduleDto enabledSchedule) => AppResources.TurnedOnShort;
    
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
        ScheduleUnitDto<TTime>? unit = units.FindByTime(timeNow);
        return unit is not null ? QuantityToString(unit.Quantity) : AppResources.TurnedOffShort;
    }

    private static string QuantityToString(QuantityDto quantity) => $"{quantity.Magnitude:F0} {quantity.Unit}";
}