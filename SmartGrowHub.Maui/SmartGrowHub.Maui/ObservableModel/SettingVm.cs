using CommunityToolkit.Mvvm.ComponentModel;
using SmartGrowHub.Domain.Model;

namespace SmartGrowHub.Maui.ObservableModel;

public sealed partial class SettingVm(
    Ulid id,
    SettingType type,
    SettingMode mode)
    : ObservableObject
{
    [ObservableProperty] private SettingMode _mode = mode;

    public Ulid Id { get; init; } = id;

    public SettingType Type { get; init; } = type;
}
