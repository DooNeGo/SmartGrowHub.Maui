using SmartGrowHub.Maui.Features.LogIn.ViewModel;

namespace SmartGrowHub.Maui.Features.LogIn.View;

public sealed partial class CheckCodePage
{
    public CheckCodePage(CheckCodePageModel pageModel)
    {
        InitializeComponent();
        BindingContext = pageModel;
    }
}