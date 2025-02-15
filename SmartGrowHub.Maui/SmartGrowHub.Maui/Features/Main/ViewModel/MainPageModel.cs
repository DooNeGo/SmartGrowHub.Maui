using System.Collections.Immutable;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using SmartGrowHub.Maui.Base;
using SmartGrowHub.Maui.Services.Api;
using SmartGrowHub.Maui.Services.App;
using SmartGrowHub.Maui.Services.Extensions;
using SmartGrowHub.Shared.GrowHubs;
using SmartGrowHub.Shared.GrowHubs.Components;

namespace SmartGrowHub.Maui.Features.Main.ViewModel;

public sealed partial class MainPageModel(
    INavigationService navigationService,
    IGrowHubService growHubService)
    : ObservableObject, IPageLifecycleAware
{
    private TaskCompletionSource<Unit> _stateChangeTcs = new(unit);

    [ObservableProperty] public partial bool IsRefreshing { get; set; }

    [ObservableProperty] public partial string? CurrentState { get; set; } = PageStates.Loading;

    [ObservableProperty] public partial bool CanStateChange { get; set; } = true;
    
    public ObservableCollection<GrowHubDto> GrowHubs { get; } = [];

    public ObservableCollection<GrowHubComponentDto> Components { get; } = [];

    public void Initialize() => RefreshAsync(CancellationToken.None).SafeFireAndForget();

    [RelayCommand]
    private async Task RefreshAsync(CancellationToken cancellationToken)
    {
        if (IsRefreshing) return;
        
        await WaitForStateChangeAsync();
        
        IsRefreshing = true;
        CurrentState = PageStates.Loading;

        Fin<ImmutableArray<GrowHubDto>> fin = await growHubService
            .GetGrowHubs(cancellationToken)
            .Map(enumerable => enumerable.ToImmutableArray())
            .RunAsync();

        fin.IfSucc(array =>
        {
            GrowHubs.Clear();
            Components.Clear();
            GrowHubs.AddRange(array);
            Components.AddRange(GrowHubs.FirstOrDefault()?.Components ?? []);
        });

        await WaitForStateChangeAsync();
        
        CurrentState = null;
        IsRefreshing = false;
    }

    [RelayCommand]
    private Task<Unit> GoToComponentsControlAsync(GrowHubComponentDto? component) =>
        component is DayLightComponentDto
            ? GoToLightControlAsync()
            : Task.FromResult(unit);

    private Task<Unit> GoToLightControlAsync() =>
        navigationService
            .NavigateAsync(Routes.LightControlPage)
            .RunAsync().AsTask();

    private Task<Unit> WaitForStateChangeAsync()
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