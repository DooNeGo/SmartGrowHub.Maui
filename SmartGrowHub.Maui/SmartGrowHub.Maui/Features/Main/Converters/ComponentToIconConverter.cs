using System.Globalization;
using SmartGrowHub.Shared.GrowHubs.Components;

namespace SmartGrowHub.Maui.Features.Main.Converters;

public sealed class ComponentToIconConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not GrowHubComponentDto component) return value;
        
        string iconName = component.Match(
            mapDayLight: _ => "sun_icon.png",
            mapUvLight: _ => "uv_light_icon.png",
            mapHeater: _ => "heat_icon.png",
            mapFan: _ => "wind_icon.png");
        
        return iconName;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}