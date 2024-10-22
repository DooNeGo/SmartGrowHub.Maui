using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Maui.ObservableModel.Extensions;

public static class PlantExtensions
{
    public static PlantVm ToVm(this Plant plant) =>
        new(plant.Id, plant.Name, plant.PlantedAt);
}
