using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SmartGrowHub.Maui.Application.Interfaces;
using SmartGrowHub.Maui.Application.Messages;
using SmartGrowHub.Maui.ObservableModel;
using System.Collections.ObjectModel;

namespace SmartGrowHub.Maui.Features.Main.ViewModel;

public sealed partial class MainPageModel : ObservableObject
{
    private readonly IDialogService _dialogService;

    [ObservableProperty] private bool _isRefreshing;
    [ObservableProperty] private ObservableCollection<GrowHubVm> _growHubs = [];

    public MainPageModel(IDialogService dialogService, IMessenger messenger)
    {
        _dialogService = dialogService;

        messenger.Register<LoggedOutMessage>(this, (_, _) =>
        {
            GrowHubs = [];
            RefreshCommand.Cancel();
            IsRefreshing = false;
        });
    }

    [RelayCommand]
    private Task RefreshAsync(CancellationToken cancellationToken)
    {
        GrowHubs.Add(new GrowHubVm(
            Ulid.NewUlid(),
            "MyFirstGrowHub",
            new PlantVm(
                Ulid.NewUlid(),
                "MyFirstPlant",
                DateTime.UtcNow),
            []));

        IsRefreshing = false;

        return Task.CompletedTask;
    }
}