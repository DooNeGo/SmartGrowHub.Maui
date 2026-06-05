using System.Globalization;
using SmartGrowHub.Shared.GrowHubs.Components;

namespace SmartGrowHub.Maui.Features.Main.Converters;

public sealed class ComponentToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not GrowHubComponentDto component) return value;
        
        string colorAsHex = component.Match(
            mapDayLight: _ => "#FFE484",
            mapUvLight: _ => "#8206ff",
            mapHeater: _ => "#f3903f",
            mapFan: _ => "#99d9ff");

        return Color.FromArgb(colorAsHex);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}