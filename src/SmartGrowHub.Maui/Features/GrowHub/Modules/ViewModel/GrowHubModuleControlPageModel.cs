using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MPowerKit.Navigation.Awares;
using MPowerKit.Navigation.Interfaces;
using MPowerKit.Navigation.Popups;
using Serilog;
using SmartGrowHub.Maui.Popups;
using SmartGrowHub.Maui.Resources.Localization;
using SmartGrowHub.Maui.Services.Api;
using SmartGrowHub.Maui.Services.Infrastructure;
using SmartGrowHub.Shared.GrowHubs.Model;
using SmartGrowHub.Shared.GrowHubs.Requests;

namespace SmartGrowHub.Maui.Features.GrowHub.Modules.ViewModel;

public sealed partial class GrowHubModuleControlPageModel : ObservableObject, IInitializeAware
{
    public const string ModuleKey = "Module";

    private readonly IGrowHubApi _growHubApi;
    private readonly IPopupNavigation _popupNavigation;
    private readonly ILogger _logger;

    public GrowHubModuleControlPageModel(
        IGrowHubApi growHubApi,
        IPopupNavigation popupNavigation,
        ILogger logger)
    {
        _growHubApi = growHubApi;
        _popupNavigation = popupNavigation;
        _logger = logger;
    }

    [ObservableProperty] public partial GrowHubModuleDto? Module { get; private set; }
    
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ScheduleModeText))]
    [NotifyPropertyChangedFor(nameof(IsScheduleEditable))]
    public partial ScheduleTypeDto SelectedMode { get; private set; } = ScheduleTypeDto.Disabled;
    
    public string ScheduleModeText => SelectedMode switch
    {
        ScheduleTypeDto.Enabled => AppResources.EnabledSchedule,
        ScheduleTypeDto.Disabled => AppResources.DisabledSchedule,
        ScheduleTypeDto.Daily => AppResources.DailySchedule,
        ScheduleTypeDto.Weekly => AppResources.WeeklySchedule,
        _ => AppResources.DisabledSchedule
    };
    
    public bool IsScheduleEditable => SelectedMode is ScheduleTypeDto.Daily or ScheduleTypeDto.Weekly;

    public ObservableCollection<ScheduleEntryModel> Entries { get; } = [];

    public void Initialize(INavigationParameters parameters)
    {
        Module = parameters.GetValue<GrowHubModuleDto?>(ModuleKey);
        if (Module is null) return;

        LoadFromSchedule(Module.Schedule);
    }

    private void LoadFromSchedule(ScheduleDto schedule)
    {
        SelectedMode = schedule.Match(
            Disabled: _ => ScheduleTypeDto.Disabled,
            Enabled: _ => ScheduleTypeDto.Enabled,
            Daily: d =>
            {
                Entries.Clear();
                foreach (ScheduleUnitDto<TimeOnly> entry in d.Entries)
                {
                    Entries.Add(new ScheduleEntryModel
                    {
                        IsDaily = true,
                        StartTime = entry.Interval.Start,
                        EndTime = entry.Interval.End,
                        Kind = entry.Kind,
                        Magnitude = entry.Quantity.Magnitude
                    });
                }
                
                return ScheduleTypeDto.Daily;
            },
            Weekly: w =>
            {
                Entries.Clear();
                foreach (ScheduleUnitDto<WeekTimeOnlyDto> entry in w.Entries)
                {
                    Entries.Add(new ScheduleEntryModel
                    {
                        IsDaily = false,
                        StartTime = entry.Interval.Start.Time,
                        EndTime = entry.Interval.End.Time,
                        StartDay = entry.Interval.Start.DayOfWeek,
                        EndDay = entry.Interval.End.DayOfWeek,
                        Kind = entry.Kind,
                        Magnitude = entry.Quantity.Magnitude
                    });
                }
                return ScheduleTypeDto.Weekly;
            });
    }

    [RelayCommand]
    private async Task SelectModeAsync()
    {
        PopupResult popupResult = await _popupNavigation.ShowAwaitablePopupAsync(Routes.ScheduleModePopupPage);
        
        if (!popupResult.Success || popupResult.Confirmation?.Confirmed is false) return;
        
        var result = popupResult.Confirmation?.CloseParameters?.GetValue<ScheduleTypeDto?>("ScheduleType");
        if (!result.HasValue) return;

        if (SelectedMode != result.Value) Entries.Clear();
        SelectedMode = result.Value;
    }

    [RelayCommand]
    private async Task AddEntryAsync()
    {
        bool isWeekly = SelectedMode == ScheduleTypeDto.Weekly;
        var popup = new ScheduleEntryEditorPopup(_popupNavigation, isWeekly);
        await _popupNavigation.ShowPopupAsync(popup);
        ScheduleEntryResult? result = await popup.WaitForResultAsync();

        if (result is null) return;

        var entry = new ScheduleEntryModel
        {
            IsDaily = !isWeekly,
            StartTime = result.StartTime,
            EndTime = result.EndTime,
            StartDay = result.StartDay,
            EndDay = result.EndDay,
            Kind = result.Kind,
            Magnitude = result.Magnitude
        };

        Entries.Add(entry);
    }

    [RelayCommand]
    private void RemoveEntry(ScheduleEntryModel entry) => Entries.Remove(entry);

    [RelayCommand]
    private async Task SaveAsync(CancellationToken cancellationToken)
    {
        if (Module is null) return;

        UpdateScheduleRequestDto request = BuildRequest(SelectedMode);

        try
        {
            await _growHubApi.UpdateScheduleAsync(Module.Schedule.Id, request, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to update schedule");
        }
    }

    private UpdateScheduleRequestDto BuildRequest(ScheduleTypeDto type) => type switch
    {
        ScheduleTypeDto.Enabled => new UpdateScheduleRequestDto(ScheduleTypeDto.Enabled),
        ScheduleTypeDto.Disabled => new UpdateScheduleRequestDto(ScheduleTypeDto.Disabled),
        ScheduleTypeDto.Daily => new UpdateScheduleRequestDto(
            ScheduleTypeDto.Daily,
            DailyEntries: Entries.Select(e => new ScheduleUnitTemplateDto<TimeOnly>(
                e.Kind,
                new QuantityDto(e.Magnitude, "%"),
                new TimeIntervalDto<TimeOnly>(e.StartTime, e.EndTime))).ToList()),
        ScheduleTypeDto.Weekly => new UpdateScheduleRequestDto(
            ScheduleTypeDto.Weekly,
            WeeklyEntries: Entries.Select(e => new ScheduleUnitTemplateDto<WeekTimeOnlyDto>(
                e.Kind,
                new QuantityDto(e.Magnitude, "%"),
                new TimeIntervalDto<WeekTimeOnlyDto>(
                    new WeekTimeOnlyDto(e.StartDay, e.StartTime),
                    new WeekTimeOnlyDto(e.EndDay, e.EndTime)))).ToList()),
        _ => new UpdateScheduleRequestDto(ScheduleTypeDto.Disabled)
    };
}