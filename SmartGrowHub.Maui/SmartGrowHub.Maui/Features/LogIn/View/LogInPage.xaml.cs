using SmartGrowHub.Maui.Features.LogIn.ViewModel;

namespace SmartGrowHub.Maui.Features.LogIn.View;

public sealed partial class LogInPage
{
    public LogInPage(LogInPageModel pageModel)
    {
        InitializeComponent();

        BindingContext = pageModel;
    }
}