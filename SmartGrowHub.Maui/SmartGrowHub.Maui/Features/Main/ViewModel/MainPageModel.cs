using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using SmartGrowHub.Maui.Services.App;
using SmartGrowHub.Shared.GrowHubs;

namespace SmartGrowHub.Maui.Features.Main.ViewModel;

public sealed partial class MainPageModel : ObservableObject
{
    private readonly IDialogService _dialogService;

    [ObservableProperty] private bool _isRefreshing;
    [ObservableProperty] private ObservableCollection<GrowHubDto> _growHubs = [];

    public MainPageModel(IDialogService dialogService)
    {
        _dialogService = dialogService;
    }

    [RelayCommand]
    private Task RefreshAsync(CancellationToken cancellationToken)
    {
        GrowHubs.Add(new GrowHubDto(Ulid.NewUlid(), "My grow hub",
            new PlantDto(Ulid.NewUlid(), "My plant", DateTime.Now)));

        IsRefreshing = false;

        return Task.CompletedTask;
    }
}