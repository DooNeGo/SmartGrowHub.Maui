using SmartGrowHub.Maui.Features.Main.ViewModel;

namespace SmartGrowHub.Maui.Features.Main.View;

public sealed partial class MainPage
{
	public MainPage(MainPageModel pageModel)
	{
		InitializeComponent();

		BindingContext = pageModel;
	}
}