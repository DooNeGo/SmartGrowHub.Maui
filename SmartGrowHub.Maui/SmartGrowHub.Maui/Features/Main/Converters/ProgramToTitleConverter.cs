using System.Globalization;
using SmartGrowHub.Maui.Resources.Localization;
using SmartGrowHub.Shared.GrowHubs.ComponentPrograms;

namespace SmartGrowHub.Maui.Features.Main.Converters;

public sealed class ProgramToTitleConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is ComponentProgramDto program
            ? program.Match(
                mapManual: _ => AppResources.ManualProgram,
                mapCycle: _ => AppResources.CycleProgram,
                mapDaily: _ => AppResources.DailyProgram,
                mapWeekly: _ => AppResources.WeeklyProgram)
            : value;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}