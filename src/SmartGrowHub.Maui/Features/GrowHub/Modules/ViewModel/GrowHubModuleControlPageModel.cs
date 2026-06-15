using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MPowerKit.Navigation.Awares;
using MPowerKit.Navigation.Interfaces;
using Serilog;
using SmartGrowHub.Maui.Services.Api;
using SmartGrowHub.Maui.Services.App;
using SmartGrowHub.Maui.Services.Infrastructure;
using SmartGrowHub.Shared.GrowHubs.Model;
using SmartGrowHub.Shared.GrowHubs.Requests;

namespace SmartGrowHub.Maui.Features.GrowHub.Modules.ViewModel;

public sealed partial class GrowHubModuleControlPageModel : ObservableObject, IInitializeAware
{
    public const string ModuleKey = "Module";
    
    private readonly IGrowHubApi _growHubApi;
    private readonly IDialogService _dialogService;
    private readonly IMainThread _mainThread;
    private readonly ILogger _logger;

    public GrowHubModuleControlPageModel(
        IGrowHubApi growHubApi,
        IDialogService dialogService,
        IMainThread mainThread,
        ILogger logger)
    {
        _growHubApi = growHubApi;
        _dialogService = dialogService;
        _mainThread = mainThread;
        _logger = logger;
    }
    
    [ObservableProperty] public partial GrowHubModuleDto? Module { get; private set; }
    [ObservableProperty] public partial bool IsEnabled { get; set; }
    
    public void Initialize(INavigationParameters parameters)
    {
        Module = parameters.GetValue<GrowHubModuleDto?>(ModuleKey);

        IsEnabled = Module?.Schedule.Match(
            Disabled: _ => false,
            Enabled: _ => true,
            Daily: _ => true,
            Weekly: _ => true) ?? false;
    }

    [RelayCommand]
    private Task<Unit> UpdateScheduleAsync(CancellationToken cancellationToken) =>
        Task.Run(() => UpdateSchedule().RunAsync(cancellationToken).AsTask(), cancellationToken);

    private IO<Unit> UpdateSchedule()
    {
        var request = new UpdateScheduleRequestDto(IsEnabled ? ScheduleTypeDto.Enabled : ScheduleTypeDto.Disabled);
        
        return _growHubApi
            .UpdateSchedule(Module.Schedule.Id, request)
            .Catch(error => IO.lift(() => _logger.Error(error.ToErrorException(), "Failed to update schedule")))
            .As();
    }

}