using SmartGrowHub.Maui.Features.LogIn.ViewModel;

namespace SmartGrowHub.Maui.Features.LogIn.View;

public sealed partial class LogInPage
{
    private readonly LogInPageModel _pageModel;

    public LogInPage(LogInPageModel pageModel)
    {
        InitializeComponent();

        BindingContext = _pageModel = pageModel;
    }
}