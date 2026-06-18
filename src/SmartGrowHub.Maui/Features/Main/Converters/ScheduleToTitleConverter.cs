using System.Globalization;
using SmartGrowHub.Maui.Resources.Localization;
using SmartGrowHub.Shared.GrowHubs.Model;

namespace SmartGrowHub.Maui.Features.Main.Converters;

public sealed class ScheduleToTitleConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value is ScheduleDto program
            ? program switch
            {
                DisabledScheduleDto => AppResources.DisabledSchedule,
                EnabledScheduleDto => AppResources.EnabledSchedule,
                DailyScheduleDto => AppResources.DailySchedule,
                WeeklyScheduleDto => AppResources.WeeklySchedule,
                _ => value
            }
            : value;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}