using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MPowerKit;
using MPowerKit.Navigation.Awares;
using MPowerKit.Navigation.Popups;
using SmartGrowHub.Shared.GrowHubs.Model;

namespace SmartGrowHub.Maui.Popups;

public sealed partial class ScheduleModePopupViewModel : ObservableObject, IPopupDialogAware
{
    public Action<(Confirmation Confirmation, bool Animated)>? RequestClose { get; set; }

    [RelayCommand]
    private void SelectMode(ScheduleTypeDto type) => RequestClose?.Invoke((new Confirmation(true,
        new NavigationParameters
        {
            ["ScheduleType"] = type
        }), true));

    [RelayCommand]
    private void Close() => RequestClose?.Invoke((new Confirmation(false, null), true));
}