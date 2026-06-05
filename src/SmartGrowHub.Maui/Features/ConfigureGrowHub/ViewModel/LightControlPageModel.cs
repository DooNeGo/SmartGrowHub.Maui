using CommunityToolkit.Mvvm.ComponentModel;

namespace SmartGrowHub.Maui.Features.ConfigureGrowHub.ViewModel;

public sealed partial class LightControlPageModel : ObservableObject
{
    [ObservableProperty] public partial double Value { get; set; }
}