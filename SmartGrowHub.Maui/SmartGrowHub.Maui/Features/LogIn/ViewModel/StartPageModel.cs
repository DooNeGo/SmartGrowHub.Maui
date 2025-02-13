using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SmartGrowHub.Maui.Services.Flow;

namespace SmartGrowHub.Maui.Features.LogIn.ViewModel;

public sealed partial class StartPageModel(ILoginByEmailService loginByEmailService) : ObservableObject
{
    [RelayCommand]
    private async Task<Unit> LogInByEmailAsync(CancellationToken cancellationToken)
    {
        await Task.Delay(100, cancellationToken).ConfigureAwait(false);
        await loginByEmailService.Start().RunAsync().AsTask().ConfigureAwait(false);
        return unit;
    }
}