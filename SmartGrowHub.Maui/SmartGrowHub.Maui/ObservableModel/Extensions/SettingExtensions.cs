using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using System.Collections.Immutable;

namespace SmartGrowHub.Maui.ObservableModel.Extensions;

public static class SettingExtensions
{
    public static SettingVm ToVm(this Setting setting) =>
        new(setting.Id, setting.Type, setting.Mode);

    public static Setting ToDomain(this SettingVm setting) =>
        new(new Id<Setting>(setting.Id), setting.Type, setting.Mode,
            ImmutableDictionary<Id<SettingComponent>, SettingComponent>.Empty);
}
