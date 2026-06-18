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
    
    private TaskCompletionSource _stateChangeTcs = new();
    
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
            _messagesService.Start(),
            RefreshAsync(CancellationToken.None)).SafeFireAndForget();
    }
    
    private void OnMeasurementReceived(SensorMeasurementDto obj) =>
        _mainThread.InvokeOnMainThreadAsync(async () =>
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
            await RefreshInternalAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.Error(e, "Failed to refresh main page");
        }
    }

    private async Task RefreshInternalAsync(CancellationToken cancellationToken)
    {
        await WaitForStateChangeAsync().ConfigureAwait(false);
        
        await _mainThread.InvokeOnMainThreadAsync(() =>
        {
            IsRefreshing = true;
            CurrentState = PageStates.Loading;
        }).ConfigureAwait(false);
        
        try
        {
            await RefreshGrowHubsAsync(cancellationToken).ConfigureAwait(false);
            
            var current = CurrentGrowHub;
            if (current is not null)
            {
                await RefreshLatestMeasurementsAsync(current.Id, cancellationToken).ConfigureAwait(false);
            }
            
            await WaitForStateChangeAsync().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Failed to refresh main page");
        }
        finally
        {
            await _mainThread.InvokeOnMainThreadAsync(() =>
            {
                CurrentState = null;
                IsRefreshing = false;
            }).ConfigureAwait(false);
        }
    }

    private async Task RefreshLatestMeasurementsAsync(string growHubId, CancellationToken cancellationToken)
    {
        var measurements = await _growHubApi.GetLatestMeasurementsAsync(growHubId, cancellationToken).ConfigureAwait(false);
        await _mainThread.InvokeOnMainThreadAsync(() =>
        {
            Measurements.Clear();
            Measurements.AddRange(measurements);
        }).ConfigureAwait(false);
    }

    private async Task RefreshGrowHubsAsync(CancellationToken cancellationToken)
    {
        var growHubs = await _growHubApi.GetGrowHubsAsync(cancellationToken).ConfigureAwait(false);
        await _mainThread.InvokeOnMainThreadAsync(() =>
        {
            GrowHubs.Clear();
            Modules.Clear();
            GrowHubs.AddRange(growHubs);
            CurrentGrowHub = GrowHubs.FirstOrDefault();
            Modules.AddRange(CurrentGrowHub?.Modules ?? []);
        }).ConfigureAwait(false);
    }

    [RelayCommand]
    private async Task GoToModuleControlAsync(GrowHubModuleDto? module)
    {
        if (module is null) return;
        await _navigationService
            .CreateBuilder(Routes.GrowHubModuleControlPage)
            .AddRouteParameter(GrowHubModuleControlPageModel.ModuleKey, module)
            .NavigateAsync()
            .ConfigureAwait(false);
    }

    private Task WaitForStateChangeAsync()
    {
        if (CanStateChange) return Task.CompletedTask;
        
        _stateChangeTcs = new TaskCompletionSource();
    
        return _stateChangeTcs.Task;
    }

    partial void OnCanStateChangeChanged(bool value)
    {
        if (!value) return;
        _stateChangeTcs.TrySetResult();
    }

    public async ValueTask DisposeAsync()
    {
        await _messagesService.Stop();
        _messagesService.MeasurementReceived -= OnMeasurementReceived;
    }
}