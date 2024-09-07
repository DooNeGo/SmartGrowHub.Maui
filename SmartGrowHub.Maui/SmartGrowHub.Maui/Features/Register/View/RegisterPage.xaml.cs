using SmartGrowHub.Maui.Features.Register.ViewModel;

namespace SmartGrowHub.Maui.Features.Register.View;

public sealed partial class RegisterPage
{
	public RegisterPage(RegisterPageModel pageModel)
	{
		InitializeComponent();

		BindingContext = pageModel;
	}
}