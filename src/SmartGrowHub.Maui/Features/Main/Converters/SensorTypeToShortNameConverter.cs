using System.Globalization;
using SmartGrowHub.Maui.Resources.Localization;
using SmartGrowHub.Shared.GrowHubs.Model;

namespace SmartGrowHub.Maui.Features.Main.Converters;

public sealed class SensorTypeToShortNameConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not SensorTypeDto sensorType) return value;

        return sensorType switch
        {
            SensorTypeDto.RandomNumber => AppResources.SensorRandomNumberShort,
            SensorTypeDto.AirTemperature => AppResources.SensorAirTemperatureShort,
            SensorTypeDto.AirPressure => AppResources.SensorAirPressureShort,
            SensorTypeDto.AirHumidity => AppResources.SensorAirHumidityShort,
            SensorTypeDto.PlantHeight => AppResources.SensorPlantHeightShort,
            SensorTypeDto.SoilMoisture => AppResources.SensorSoilMoistureShort,
            SensorTypeDto.SoilTemperature => AppResources.SensorSoilTemperatureShort,
            SensorTypeDto.Illumination => AppResources.SensorIlluminationShort,
            _ => sensorType.ToString()
        };
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value;
    }
}

