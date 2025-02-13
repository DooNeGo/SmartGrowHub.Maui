using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGrowHub.Maui.Services.Flow;

namespace SmartGrowHub.Maui.Features.LogIn.ViewModel;

public sealed partial class StartPageModel(ILoginByEmailService loginByEmailService) : ObservableObject
{
    [RelayCommand]
    private Task<Unit> LogInByEmailAsync(CancellationToken cancellationToken) =>
        loginByEmailService.Start().RunAsync().AsTask();
}