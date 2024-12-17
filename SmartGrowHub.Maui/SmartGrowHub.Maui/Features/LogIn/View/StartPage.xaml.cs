using SmartGrowHub.Maui.Features.LogIn.ViewModel;

namespace SmartGrowHub.Maui.Features.LogIn.View;

public sealed partial class StartPage
{
    public StartPage(StartPageModel pageModel)
    {
        InitializeComponent();

        BindingContext = pageModel;
    }
}