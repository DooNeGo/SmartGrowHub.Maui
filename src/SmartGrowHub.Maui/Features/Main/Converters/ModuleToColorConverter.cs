using System.Globalization;
using SmartGrowHub.Shared.GrowHubs.Model;

namespace SmartGrowHub.Maui.Features.Main.Converters;

public sealed class ModuleToColorConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not GrowHubModuleDto module) return value;

        string colorAsHex = module.Type switch
        {
            ModuleTypeDto.DayLight => "#FFE484",
            ModuleTypeDto.UvLight => "#8206ff",
            ModuleTypeDto.Heater => "#f3903f",
            ModuleTypeDto.Fan => "#99d9ff",
            ModuleTypeDto.Led => "#ffffff",
            ModuleTypeDto.Humidifier => "#a0d2eb",
            ModuleTypeDto.WaterPump => "#4b70b8",
            ModuleTypeDto.AirFlap => "#50c878",
            _ => throw new ArgumentOutOfRangeException(nameof(value), module.Type, null)
        };

        return Color.FromArgb(colorAsHex);
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}