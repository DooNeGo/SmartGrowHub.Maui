using CommunityToolkit.Mvvm.ComponentModel;
using MPowerKit.Navigation.Awares;
using MPowerKit.Navigation.Interfaces;
using SmartGrowHub.Shared.GrowHubs.Model;

namespace SmartGrowHub.Maui.Features.GrowHub.Modules.ViewModel;

public sealed partial class GrowHubModuleControlPageModel : ObservableObject, IInitializeAware
{
    public const string ModuleKey = "Module";
    
    [ObservableProperty] public partial GrowHubModuleDto? Module { get; private set; }
    
    public void Initialize(INavigationParameters parameters)
    {
        Module = parameters.GetValue<GrowHubModuleDto?>(ModuleKey);
    }
}