using System.Globalization;
using SmartGrowHub.Maui.Resources.Localization;
using SmartGrowHub.Shared.GrowHubs.Model;

namespace SmartGrowHub.Maui.Features.Main.Converters;

public sealed class ModuleTypeToNameConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not ModuleTypeDto moduleType) return value;

        return moduleType switch
        {
            ModuleTypeDto.DayLight => AppResources.DayLight,
            ModuleTypeDto.Fan => AppResources.Fan,
            ModuleTypeDto.Heater => AppResources.Heater,
            ModuleTypeDto.Led => AppResources.ModuleTypeLed,
            ModuleTypeDto.UvLight => AppResources.ModuleTypeUvLight,
            ModuleTypeDto.Humidifier => AppResources.ModuleTypeHumidifier,
            ModuleTypeDto.WaterPump => AppResources.ModuleTypeWaterPump,
            ModuleTypeDto.AirFlap => AppResources.ModuleTypeAirFlap,
            _ => moduleType.ToString()
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}

