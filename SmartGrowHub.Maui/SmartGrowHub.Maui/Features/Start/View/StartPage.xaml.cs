using SmartGrowHub.Maui.Features.Start.ViewModel;

namespace SmartGrowHub.Maui.Features.Start.View;

public sealed partial class StartPage
{
    public StartPage(StartPageModel pageModel)
    {
        InitializeComponent();

        BindingContext = pageModel;
    }
}