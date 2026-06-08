using System.Collections.Immutable;
using System.Collections.ObjectModel;
using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MPowerKit.Navigation.Awares;
using MPowerKit.Navigation.Interfaces;
using Serilog;
using SmartGrowHub.Maui.Base;
using SmartGrowHub.Maui.Services.Api;
using SmartGrowHub.Shared.GrowHubs.Model;
using INavigationService = SmartGrowHub.Maui.Services.App.INavigationService;

namespace SmartGrowHub.Maui.Features.Main.ViewModel;

public sealed partial class MainPageModel : ObservableObject, IInitializeAware
{
    private TaskCompletionSource<Unit> _stateChangeTcs = new(Unit.Default);
    
    private readonly INavigationService _navigationService;
    private readonly IGrowHubService _growHubService;
    private readonly ILogger _logger;

    public MainPageModel(
        INavigationService navigationService,
        IGrowHubService growHubService,
        ILogger logger)
    {
        _navigationService = navigationService;
        _growHubService = growHubService;
        _logger = logger;
    }

    [ObservableProperty] public partial bool IsRefreshing { get; set; }
    [ObservableProperty] public partial string? CurrentState { get; set; } = PageStates.Loading;
    [ObservableProperty] public partial bool CanStateChange { get; set; } = true;
    [ObservableProperty] public partial GrowHubDto? CurrentGrowHub { get; private set; }
    [ObservableProperty] public partial ImmutableList<SensorMeasurementDto> Measurements { get; private set; } = [];
    
    public ObservableCollection<GrowHubDto> GrowHubs { get; } = [];

    public ObservableCollection<GrowHubModuleDto> Modules { get; } = [];

    public void Initialize(INavigationParameters parameters) =>
        RefreshAsync(CancellationToken.None).SafeFireAndForget();

    [RelayCommand]
    private async Task RefreshAsync(CancellationToken cancellationToken)
    {
        if (IsRefreshing) return;

        await WaitForStateChangeAsync();

        IsRefreshing = true;
        CurrentState = PageStates.Loading;

        _ = await RefreshGrowHubs()
            .RunSafeAsync(EnvIO.New(token: cancellationToken))
            .Map(fin => fin.IfFail(error => _logger.Error(error.ToException(), "Failed to refresh grow hubs")));

        if (CurrentGrowHub is not null)
        {
            _ = await RefreshLatestMeasurements(CurrentGrowHub.Id)
                .RunSafeAsync(EnvIO.New(token: cancellationToken))
                .Map(fin => fin.IfFail(error =>
                    _logger.Error(error.ToException(), "Failed to refresh latest measurements")));
        }

        await WaitForStateChangeAsync();

        CurrentState = null;
        IsRefreshing = false;
    }
    
    private IO<Unit> RefreshLatestMeasurements(string growHubId) =>
        from measurements in _growHubService.GetLatestMeasurements(growHubId)
        from _ in IO.lift(() => Measurements = measurements)
        select Unit.Default;
    
    private IO<Unit> RefreshGrowHubs() =>
        from growHubs in _growHubService.GetGrowHubs()
        from _ in IO.lift(() =>
        {
            GrowHubs.Clear();
            Modules.Clear();
            GrowHubs.AddRange(growHubs);
            CurrentGrowHub = GrowHubs.FirstOrDefault();
            Modules.AddRange(CurrentGrowHub?.Modules ?? []);
        })
        select _;

    [RelayCommand]
    private Task<Unit> GoToComponentsControlAsync(GrowHubModuleDto? module) =>
        module?.Type is ModuleTypeDto.DayLight
            ? GoToLightControlAsync()
            : Task.FromResult(Unit.Default);

    private Task<Unit> GoToLightControlAsync() =>
        _navigationService
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