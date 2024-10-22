using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Immutable;

namespace SmartGrowHub.Maui.ObservableModel;

public sealed partial class GrowHubVm(
    Ulid id,
    string name,
    PlantVm? plant,
    ImmutableArray<SettingVm> settings)
    : ObservableObject
{
    [ObservableProperty] private string _name = name;
    [ObservableProperty] private PlantVm? _plant = plant;

    public Ulid Id { get; init; } = id;

    public ImmutableArray<SettingVm> Settings { get; init; } = settings;
}
