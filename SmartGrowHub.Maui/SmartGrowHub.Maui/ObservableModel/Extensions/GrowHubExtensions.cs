using LanguageExt.UnsafeValueAccess;
using SmartGrowHub.Domain.Common;
using SmartGrowHub.Domain.Model;
using System.Collections.Immutable;

namespace SmartGrowHub.Maui.ObservableModel.Extensions;

public static class GrowHubExtensions
{
    public static GrowHubVm ToVm(this GrowHub growHub) =>
        new(growHub.Id, growHub.Name, growHub.Plant.ValueUnsafe()?.ToVm(), growHub.Settings.ToVm());

    public static ImmutableArray<SettingVm> ToVm(this ImmutableDictionary<Id<Setting>, Setting> settings) =>
        settings.Select(keyValuePair => keyValuePair.Value.ToVm()).ToImmutableArray();
}
