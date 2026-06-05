using System.Globalization;
using SmartGrowHub.Shared.GrowHubs.Model;

namespace SmartGrowHub.Maui.Features.Main.Converters;

public sealed class ModuleToIconConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not GrowHubModuleDto module) return value;

        return module.Type switch
        {
            ModuleTypeDto.DayLight => "sun_icon.png",
            ModuleTypeDto.UvLight => "uv_light_icon.png",
            ModuleTypeDto.Heater => "heat_icon.png",
            ModuleTypeDto.Fan => "wind_icon.png",
            ModuleTypeDto.Led => "led_icon.png",
            ModuleTypeDto.Humidifier => "humidifier_icon.png",
            ModuleTypeDto.WaterPump => "water_pump_icon.png",
            ModuleTypeDto.AirFlap => "air_flap_icon.png",
            _ => throw new ArgumentOutOfRangeException(nameof(value), module.Type, null)
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}