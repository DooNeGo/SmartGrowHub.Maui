using SmartGrowHub.Maui.Features.UserProfile.ViewModel;

namespace SmartGrowHub.Maui.Features.UserProfile.View;

public sealed partial class UserProfilePage
{
    public UserProfilePage(UserProfilePageModel pageModel)
	{
		InitializeComponent();
		BindingContext = pageModel;
	}
}