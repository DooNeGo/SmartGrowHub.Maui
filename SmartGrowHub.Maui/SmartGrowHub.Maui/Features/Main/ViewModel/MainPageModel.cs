using System.Collections.Immutable;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AsyncAwaitBestPractices;
using SmartGrowHub.Maui.Base;
using SmartGrowHub.Maui.Features.ConfigureGrowHub.ViewModel;
using SmartGrowHub.Maui.Resources.Localization;
using SmartGrowHub.Maui.Services.Api;
using SmartGrowHub.Maui.Services.App;
using SmartGrowHub.Shared.GrowHubs;
using SmartGrowHub.Shared.GrowHubs.Components;
using SmartGrowHub.Shared.GrowHubs.Settings;

namespace SmartGrowHub.Maui.Features.Main.ViewModel;

public sealed partial class MainPageModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private readonly IGrowHubService _growHubService;
    
    private TaskCompletionSource<bool> _stateChangeTcs = new(true);

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

    public MainPageModel(INavigationService navigationService, IGrowHubService growHubService)
    {
        _navigationService = navigationService;
        _growHubService = growHubService;
        
        Refresh().SafeFireAndForget();
        
        return;

        async Task Refresh()
        {
            await Task.Delay(500);
            await RefreshAsync(CancellationToken.None);
        }
    }

    [RelayCommand]
    private async Task RefreshAsync(CancellationToken cancellationToken)
    {
        if (IsRefreshing) return;
        
        await WaitForStateChange();
        
        IsRefreshing = true;
        CurrentState = PageStates.Loading;
        
        ImmutableArray<GrowHubDto> response = await _growHubService
            .GetGrowHubs(cancellationToken)
            .RunUnsafeAsync()
            .Map(enumerable => enumerable.ToImmutableArray());
        
        GrowHubs.Clear();
        GrowHubs.AddRange(response);

        UpdateData(response);

        await WaitForStateChange();
        
        CurrentState = null;
        IsRefreshing = false;
    }

    private void UpdateData(ImmutableArray<GrowHubDto> response)
    {
        if (response.Length is 0) return;
        
        foreach (IGrowHubComponentDto component in response.FirstOrDefault()!.Components)
        {
            if (component is LightComponentDto light)
            {
                LightState = GetSettingState(light.Setting);
                LightValue = $"{GetSettingValue(light.Setting)} %";
            }
            else if (component is FanComponentDto fan)
            {
                FanState = GetSettingState(fan.Setting);
                FanValue = $"{GetSettingValue(fan.Setting)} %";
            }
            else if (component is HeaterComponentDto heater)
            {
                HeatState = GetSettingState(heater.Setting);
                HeatValue = $"{GetSettingValue(heater.Setting)} %";
            }
        }
    }

    private static string GetSettingState(ISettingDto setting) =>
        setting.Match(_ => AppResources.Manual, _ => AppResources.Cycle);
    
    private static int GetSettingValue(ISettingDto setting) =>
        setting.Match(manual => manual.Value, cycle => cycle.Value);

    [RelayCommand]
    private Task<Unit> GoToLightControl(CancellationToken cancellationToken) =>
        _navigationService
            .GoToAsync(nameof(LightControlPageModel), cancellationToken)
            .RunAsync().AsTask();

    private Task<bool> WaitForStateChange()
    {
        _stateChangeTcs = new TaskCompletionSource<bool>();

        if (CanStateChange)
        {
            _stateChangeTcs.TrySetResult(true);
        }
    
        return _stateChangeTcs.Task;
    }

    partial void OnCanStateChangeChanged(bool value)
    {
        if (value)
        {
            _stateChangeTcs.TrySetResult(true);
        }
    }
}