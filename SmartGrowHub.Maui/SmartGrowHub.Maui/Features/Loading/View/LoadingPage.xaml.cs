using SmartGrowHub.Maui.Features.Loading.ViewModel;

namespace SmartGrowHub.Maui.Features.Loading.View;

public sealed partial class LoadingPage
{
	public LoadingPage(LoadingPageModel pageModel)
	{
		InitializeComponent();
		BindingContext = pageModel;
	}
}