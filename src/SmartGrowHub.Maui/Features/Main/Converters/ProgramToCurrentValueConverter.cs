using System.Globalization;
using SmartGrowHub.Maui.Resources.Localization;
using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Shared.GrowHubs;
using SmartGrowHub.Shared.GrowHubs.ComponentPrograms;

namespace SmartGrowHub.Maui.Features.Main.Converters;

public sealed class ProgramToCurrentValueConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is ComponentProgramDto program
            ? program.Match(MapManualToString, MapCycleToString, MapDailyToString, MapWeeklyToString)
            : value;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
    
    private static string MapManualToString(ManualProgramDto manualProgram) =>
        QuantityToString(manualProgram.Quantity);

    private static string MapCycleToString(CycleProgramDto cycleProgram) =>
        cycleProgram.CycleParameters.Interval.Contains(TimeOnly.FromDateTime(DateTime.Now))
            ? QuantityToString(cycleProgram.CycleParameters.Quantity)
            : AppResources.TurnOffShort;
    
    private static string MapDailyToString(DailyProgramDto dailyProgram) =>
        GetFormattedCurrentValue(dailyProgram, TimeOnly.FromDateTime(DateTime.Now));

    private static string MapWeeklyToString(WeeklyProgramDto weeklyProgram)
    {
        DateTime dateTimeNow = DateTime.Now;
        TimeOnly timeNow = TimeOnly.FromDateTime(dateTimeNow);
        WeekTimeOnlyDto weekTime = new(dateTimeNow.DayOfWeek, timeNow);
        
        return GetFormattedCurrentValue(weeklyProgram, weekTime);
    }

    private static string GetFormattedCurrentValue<TTime>(
        IntervalProgramDto<TTime> intervalProgram, in TTime timeNow) where TTime : IComparable<TTime> =>
        intervalProgram.Entries
            .FindByTime(timeNow)
            .Map(timedQuantity => QuantityToString(timedQuantity.Quantity))
            .IfNone(() => AppResources.TurnOffShort);

    private static string QuantityToString(QuantityDto quantity) => $"{quantity.Magnitude} {quantity.Unit}";
}