using SmartGrowHub.Maui.Features.AppStart.ViewModel;

namespace SmartGrowHub.Maui.Features.AppStart.View;

public sealed partial class AppStartPage
{
	public AppStartPage(AppStartPageModel pageModel)
	{
		InitializeComponent();
		BindingContext = pageModel;
	}
}