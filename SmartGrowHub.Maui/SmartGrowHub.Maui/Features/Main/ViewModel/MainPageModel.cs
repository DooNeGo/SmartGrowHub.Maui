using System.Collections.Immutable;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AsyncAwaitBestPractices;
using MPowerKit.Navigation.Awares;
using MPowerKit.Navigation.Interfaces;
using SmartGrowHub.Maui.Base;
using SmartGrowHub.Maui.Services.Api;
using SmartGrowHub.Shared.GrowHubs;
using SmartGrowHub.Shared.GrowHubs.Components;
using INavigationService = SmartGrowHub.Maui.Services.App.INavigationService;

namespace SmartGrowHub.Maui.Features.Main.ViewModel;

public sealed partial class MainPageModel(
    INavigationService navigationService,
    IGrowHubService growHubService)
    : ObservableObject, IInitializeAware
{
    private TaskCompletionSource<Unit> _stateChangeTcs = new(Unit.Default);

    [ObservableProperty] public partial bool IsRefreshing { get; set; }

    [ObservableProperty] public partial string? CurrentState { get; set; } = PageStates.Loading;

    [ObservableProperty] public partial bool CanStateChange { get; set; } = true;
    
    public ObservableCollection<GrowHubDto> GrowHubs { get; } = [];

    public ObservableCollection<GrowHubComponentDto> Components { get; } = [];

    public void Initialize(INavigationParameters parameters) =>
        RefreshAsync(CancellationToken.None).SafeFireAndForget();

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
            .RunSafeAsync();

        fin.ThrowIfFail();

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
            : Task.FromResult(Unit.Default);

    private Task<Unit> GoToLightControlAsync() =>
        navigationService
            .NavigateAsync(Routes.LightControlPage)
            .RunAsync().AsTask();

    private Task<Unit> WaitForStateChangeAsync()
    {
        if (CanStateChange) return Task.FromResult(Unit.Default);
        
        _stateChangeTcs = new TaskCompletionSource<Unit>();
    
        return _stateChangeTcs.Task;
    }

    partial void OnCanStateChangeChanged(bool value)
    {
        if (!value) return;
        _stateChangeTcs.TrySetResult(Unit.Default);
    }
}