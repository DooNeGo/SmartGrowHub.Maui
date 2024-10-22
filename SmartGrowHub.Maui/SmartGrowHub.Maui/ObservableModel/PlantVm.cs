using CommunityToolkit.Mvvm.ComponentModel;

namespace SmartGrowHub.Maui.ObservableModel;

public sealed partial class PlantVm(
    Ulid id,
    string name,
    DateTime plantedAt)
    : ObservableObject
{
    [ObservableProperty] private string _name = name;

    public Ulid Id { get; init; } = id;

    public DateTime PlantedAt { get; init; } = plantedAt;
}
