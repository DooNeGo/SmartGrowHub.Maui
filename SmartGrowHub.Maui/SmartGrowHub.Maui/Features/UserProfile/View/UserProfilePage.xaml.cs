using SmartGrowHub.Maui.Features.UserProfile.ViewModel;

namespace SmartGrowHub.Maui.Features.UserProfile.View;

public sealed partial class UserProfilePage
{
	private readonly UserProfilePageModel _pageModel;

    public UserProfilePage(UserProfilePageModel pageModel)
	{
		InitializeComponent();

		BindingContext = _pageModel = pageModel;
	}
}