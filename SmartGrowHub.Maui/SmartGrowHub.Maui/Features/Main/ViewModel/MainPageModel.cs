using System.Collections.Immutable;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using SmartGrowHub.Maui.Base;
using SmartGrowHub.Maui.Resources.Localization;
using SmartGrowHub.Maui.Services.Api;
using SmartGrowHub.Maui.Services.App;
using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Maui.Services.Infrastructure;
using SmartGrowHub.Shared.GrowHubs;
using SmartGrowHub.Shared.GrowHubs.ComponentPrograms;

namespace SmartGrowHub.Maui.Features.Main.ViewModel;

public sealed partial class MainPageModel(
    INavigationService navigationService,
    IGrowHubService growHubService,
    ITimeProvider timeProvider)
    : ObservableObject, IPageLifecycleAware
{
    private TaskCompletionSource<Unit> _stateChangeTcs = new(unit);

    [ObservableProperty] private bool _isRefreshing;
    
    [ObservableProperty] private ObservableCollection<GrowHubDto> _growHubs = [];
    
    [ObservableProperty] private string _lightState = string.Empty;
    [ObservableProperty] private string _fanState = string.Empty;
    [ObservableProperty] private string _heatState = string.Empty;
    
    [ObservableProperty] private string _lightValue = string.Empty;
    [ObservableProperty] private string _fanValue = string.Empty;
    [ObservableProperty] private string _heatValue = string.Empty;
    
    [ObservableProperty] private string? _currentState = PageStates.Loading;
    [ObservableProperty] private bool _canStateChange = true;

    public void Initialize() => RefreshAsync(CancellationToken.None).SafeFireAndForget();

    [RelayCommand]
    private async Task RefreshAsync(CancellationToken cancellationToken)
    {
        if (IsRefreshing) return;
        
        await WaitForStateChange();
        
        IsRefreshing = true;
        CurrentState = PageStates.Loading;

        _ = await growHubService
            .GetGrowHubs(cancellationToken)
            .Map(enumerable => enumerable.ToImmutableArray())
            .Bind(array => liftEff(() =>
            {
                GrowHubs.Clear();
                GrowHubs.AddRange(array);
                return UpdateData(array);
            }))
            .RunAsync();

        await WaitForStateChange();
        
        CurrentState = null;
        IsRefreshing = false;
    }

    private Unit UpdateData(IEnumerable<GrowHubDto> response)
    {
        GrowHubDto? growHub = response.FirstOrDefault();
        if (growHub is null) return unit;
        
        LightState = GetProgramState(growHub.DayLightComponent.Program);
        LightValue = GetProgramValue(growHub.DayLightComponent.Program);
        
        FanState = GetProgramState(growHub.FanComponent.Program);
        FanValue = GetProgramValue(growHub.FanComponent.Program);
        
        HeatState = GetProgramState(growHub.HeaterComponent.Program);
        HeatValue = GetProgramValue(growHub.HeaterComponent.Program);

        return unit;
    }

    private static string GetProgramState(ComponentProgramDto program) =>
        program.Match(
            mapManual: _ => AppResources.Manual,
            mapCycle: _ => AppResources.Cycle,
            mapDaily: _ => AppResources.DailyProgram,
            mapWeekly: _ => AppResources.WeeklyProgram);
    
    private string GetProgramValue(ComponentProgramDto program) =>
        program.Match(
            mapManual: manual => QuantityToString(manual.Quantity),
            mapCycle: cycle => QuantityToString(cycle.CycleParameters.Quantity),
            mapDaily: daily => MapDailyToString(daily).Run(),
            mapWeekly: weekly => MapWeeklyToString(weekly).Run());

    private IO<string> MapDailyToString(DailyProgramDto dailyProgram) =>
        timeProvider.Now
            .Map(TimeOnly.FromDateTime)
            .Map(time => GetIntervalProgramCurrentQuantity(dailyProgram, time));
    
    private IO<string> MapWeeklyToString(WeeklyProgramDto weeklyProgram) =>
        timeProvider.Now
            .Map(dateTime => new WeekTimeOnlyDto(dateTime.DayOfWeek, TimeOnly.FromDateTime(dateTime)))
            .Map(time => GetIntervalProgramCurrentQuantity(weeklyProgram, time));

    private static string GetIntervalProgramCurrentQuantity<TTime>(
        IntervalProgramDto<TTime> intervalProgram, TTime timeNow) where TTime : IComparable<TTime> =>
        FindByTime(intervalProgram.Entries, timeNow)
            .Map(timedQuantity => QuantityToString(timedQuantity.Quantity)).As()
            .IfNone(() => AppResources.TurnOffShort);

    private static string QuantityToString(QuantityDto quantity) => $"{quantity.Magnitude} {quantity.Unit}";

    private static Option<TimedQuantityDto<TTime>> FindByTime<TTime>(
        IEnumerable<TimedQuantityDto<TTime>> timedQuantities, TTime time) where TTime : IComparable<TTime> =>
        Optional(timedQuantities.FirstOrDefault(timedQuantity => timedQuantity.Interval.Contains(time)));

    [RelayCommand]
    private Task<Unit> GoToLightControl(CancellationToken cancellationToken) =>
        navigationService
            .NavigateAsync(Routes.LightControlPage)
            .RunAsync().AsTask();

    private Task<Unit> WaitForStateChange()
    {
        if (CanStateChange) return Task.FromResult(unit);
        
        _stateChangeTcs = new TaskCompletionSource<Unit>();
    
        return _stateChangeTcs.Task;
    }

    partial void OnCanStateChangeChanged(bool value)
    {
        if (!value) return;
        _stateChangeTcs.TrySetResult(unit);
    }
}