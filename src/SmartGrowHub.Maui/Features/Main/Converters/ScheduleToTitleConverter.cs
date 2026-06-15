using System.Globalization;
using SmartGrowHub.Maui.Resources.Localization;
using SmartGrowHub.Shared.GrowHubs.Model;

namespace SmartGrowHub.Maui.Features.Main.Converters;

public sealed class ScheduleToTitleConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is ScheduleDto program
            ? program.Match(
                Disabled: _ => AppResources.DisabledSchedule,
                Enabled: _ => AppResources.EnabledSchedule,
                Daily: _ => AppResources.DailySchedule,
                Weekly: _ => AppResources.WeeklySchedule)
            : value;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}