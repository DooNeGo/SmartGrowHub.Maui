using SmartGrowHub.Maui.Features.LogIn.ViewModel;

namespace SmartGrowHub.Maui.Features.LogIn.View;

public sealed partial class LoginByEmailPage
{
    public LoginByEmailPage(LoginByEmailPageModel pageModel)
    {
        InitializeComponent();
        BindingContext =  pageModel;
    }
}