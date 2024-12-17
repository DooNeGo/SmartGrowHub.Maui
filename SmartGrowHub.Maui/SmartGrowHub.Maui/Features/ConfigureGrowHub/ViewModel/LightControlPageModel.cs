using CommunityToolkit.Mvvm.ComponentModel;

namespace SmartGrowHub.Maui.Features.ConfigureGrowHub.ViewModel;

public sealed partial class LightControlPageModel : ObservableObject
{
    [ObservableProperty] private double _value;
}