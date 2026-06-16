using CommunityToolkit.Mvvm.ComponentModel;
using SmartGrowHub.Maui.Resources.Localization;
using SmartGrowHub.Shared.GrowHubs.Model;

namespace SmartGrowHub.Maui.Features.GrowHub.Modules.ViewModel;

public sealed partial class ScheduleEntryModel : ObservableObject
{
    public bool IsDaily { get; init; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayText), nameof(TimeDisplayText))]
    public partial TimeOnly StartTime { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayText), nameof(TimeDisplayText))]
    public partial TimeOnly EndTime { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayText), nameof(TimeDisplayText))]
    public partial DayOfWeek StartDay { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayText), nameof(TimeDisplayText))]
    public partial DayOfWeek EndDay { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayText), nameof(TimeDisplayText))]
    public partial ScheduleUnitKindDto Kind { get; set; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DisplayText), nameof(TimeDisplayText))]
    public partial float Magnitude { get; set; }

    public string DisplayText => $"{KindLabel} {Magnitude:F0}%";

    public string TimeDisplayText => IsDaily
        ? $"{StartTime:HH:mm} – {EndTime:HH:mm}"
        : $"{StartDay:G} {StartTime:HH:mm} – {EndDay:G} {EndTime:HH:mm}";

    private string KindLabel => Kind is ScheduleUnitKindDto.Power
        ? AppResources.Power
        : AppResources.Prefer;
}