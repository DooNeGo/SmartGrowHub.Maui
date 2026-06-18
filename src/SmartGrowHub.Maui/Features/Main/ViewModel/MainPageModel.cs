using System.Collections.ObjectModel;
using AsyncAwaitBestPractices;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MPowerKit.Navigation.Awares;
using MPowerKit.Navigation.Interfaces;
using Serilog;
using SmartGrowHub.Maui.Base;
using SmartGrowHub.Maui.Features.GrowHub.Modules.ViewModel;
using SmartGrowHub.Maui.Services.Api;
using SmartGrowHub.Maui.Services.Infrastructure;
using SmartGrowHub.Shared.GrowHubs.Model;
using INavigationService = SmartGrowHub.Maui.Services.App.INavigationService;

namespace SmartGrowHub.Maui.Features.Main.ViewModel;

public sealed partial class MainPageModel : ObservableObject, IInitializeAware, IAsyncDisposable
{
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    
    private TaskCompletionSource<Unit> _stateChangeTcs = new(Unit.Default);
    
    private readonly INavigationService _navigationService;
    private readonly IMessagesService _messagesService;
    private readonly IGrowHubApi _growHubApi;
    private readonly IMainThread _mainThread;
    private readonly ILogger _logger;

    public MainPageModel(
        INavigationService navigationService,
        IMessagesService messagesService,
        IGrowHubApi growHubApi,
        IMainThread mainThread,
        ILogger logger)
    {
        _navigationService = navigationService;
        _messagesService = messagesService;
        _growHubApi = growHubApi;
        _mainThread = mainThread;
        _logger = logger;
    }

    [ObservableProperty] public partial bool IsRefreshing { get; set; }
    [ObservableProperty] public partial string? CurrentState { get; set; } = PageStates.Loading;
    [ObservableProperty] public partial bool CanStateChange { get; set; } = true;
    [ObservableProperty] public partial GrowHubDto? CurrentGrowHub { get; private set; }
    
    public ObservableCollection<SensorMeasurementDto> Measurements { get; } = [];
    
    public ObservableCollection<GrowHubDto> GrowHubs { get; } = [];

    public ObservableCollection<GrowHubModuleDto> Modules { get; } = [];

    public void Initialize(INavigationParameters parameters)
    {
        _messagesService.MeasurementReceived += OnMeasurementReceived;
        
        Task.WhenAll(
            Task.Run(() => _messagesService.Start().RunAsync().AsTask()),
            RefreshAsync(CancellationToken.None)).SafeFireAndForget();
    }
    
    private void OnMeasurementReceived(SensorMeasurementDto obj) =>
        MainThread.InvokeOnMainThreadAsync(async () =>
        {
            await _semaphore.WaitAsync(CancellationToken.None);
            
            try
            {
                SensorMeasurementDto? measurement = Measurements.FirstOrDefault(m =>
                    string.Equals(m.SensorId, obj.SensorId, StringComparison.Ordinal));
                
                if (measurement is null)
                {
                    Measurements.Add(obj);
                    return;
                }
                
                int index = Measurements.IndexOf(measurement);
                Measurements.RemoveAt(index);
                Measurements.Insert(index, obj);
            }
            finally
            {
                _semaphore.Release();
            }
        }).SafeFireAndForget();

    [RelayCommand]
    private async Task RefreshAsync(CancellationToken cancellationToken)
    {
        if (IsRefreshing) return;

        try
        {
            await Task.Run(() => Refresh.RunAsync(cancellationToken), cancellationToken);
        }
        catch (Exception e)
        {
            _logger.Error(e, "Failed to refresh main page");
        }
    }

    private IO<Unit> Refresh => (
            from _1 in WaitForStateChange
            from _2 in _mainThread.InvokeOnMainThread(() =>
            {
                IsRefreshing = true;
                CurrentState = PageStates.Loading;
            })
            from _3 in RefreshGrowHubs
            from current in IO.lift(() => CurrentGrowHub)
            from _4 in current is null
                ? IO.pure(Unit.Default)
                : RefreshLatestMeasurements(current.Id)
            from _5 in WaitForStateChange
            select _5)
        .Catch(error => IO.lift(() => _logger.Error(error.ToErrorException(), "Failed to refresh main page")))
        .Finally(_mainThread.InvokeOnMainThread(() =>
        {
            CurrentState = null;
            IsRefreshing = false;
        })).As();
        

    private IO<Unit> RefreshLatestMeasurements(string growHubId) =>
        from measurements in _growHubApi.GetLatestMeasurements(growHubId)
        from _ in _mainThread.InvokeOnMainThread(() =>
        {
            Measurements.Clear();
            Measurements.AddRange(measurements);
        })
        select Unit.Default;

    private IO<Unit> RefreshGrowHubs =>
        from growHubs in _growHubApi.GetGrowHubs()
        from _ in _mainThread.InvokeOnMainThread(() =>
        {
            GrowHubs.Clear();
            Modules.Clear();
            GrowHubs.AddRange(growHubs);
            CurrentGrowHub = GrowHubs.FirstOrDefault();
            Modules.AddRange(CurrentGrowHub?.Modules ?? []);
        })
        select _;

    [RelayCommand]
    private Task<Unit> GoToModuleControlAsync(GrowHubModuleDto? module) =>
        _navigationService
            .CreateBuilder(Routes.GrowHubModuleControlPage)
            .AddRouteParameter(GrowHubModuleControlPageModel.ModuleKey, module!)
            .Navigate()
            .RunAsync().AsTask();

    private Task<Unit> WaitForStateChangeAsync()
    {
        if (CanStateChange) return Task.FromResult(Unit.Default);
        
        _stateChangeTcs = new TaskCompletionSource<Unit>();
    
        return _stateChangeTcs.Task;
    }

    private IO<Unit> WaitForStateChange => IO.liftAsync(WaitForStateChangeAsync);

    partial void OnCanStateChangeChanged(bool value)
    {
        if (!value) return;
        _stateChangeTcs.TrySetResult(Unit.Default);
    }

    public async ValueTask DisposeAsync()
    {
        await Task.Run(() => _messagesService.Stop().RunAsync().AsTask());
        _messagesService.MeasurementReceived -= OnMeasurementReceived;
    }
}