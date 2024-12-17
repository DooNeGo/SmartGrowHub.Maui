using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using AsyncAwaitBestPractices;
using SmartGrowHub.Maui.Base;
using SmartGrowHub.Maui.Features.ConfigureGrowHub.ViewModel;
using SmartGrowHub.Maui.Services.App;
using SmartGrowHub.Shared.GrowHubs;

namespace SmartGrowHub.Maui.Features.Main.ViewModel;

public sealed partial class MainPageModel : ObservableObject
{
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigationService;

    [ObservableProperty] private bool _isRefreshing;
    [ObservableProperty] private ObservableCollection<GrowHubDto> _growHubs = [];
    [ObservableProperty] private string? _currentState = PageStates.Loading;

    public MainPageModel(IDialogService dialogService, INavigationService navigationService)
    {
        _dialogService = dialogService;
        _navigationService = navigationService;

        GrowHubs.Add(new GrowHubDto(Ulid.NewUlid(), "My grow hub",
            new PlantDto(Ulid.NewUlid(), "My plant", DateTime.Now)));
        
        RefreshAsync(CancellationToken.None).SafeFireAndForget();
    }

    [RelayCommand]
    private async Task RefreshAsync(CancellationToken cancellationToken)
    {
        if (IsRefreshing) return;
        
        IsRefreshing = true;

        CurrentState = PageStates.Loading;
        await Task.Delay(1500, cancellationToken);
        CurrentState = null;
        
        IsRefreshing = false;
    }

    [RelayCommand]
    private Task<Unit> GoToLightControl(CancellationToken cancellationToken) =>
        _navigationService
            .GoToAsync(nameof(LightControlPageModel), cancellationToken)
            .RunAsync().AsTask();
}