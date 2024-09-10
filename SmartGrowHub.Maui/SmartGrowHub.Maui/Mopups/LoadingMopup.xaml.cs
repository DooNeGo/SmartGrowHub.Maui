using AsyncAwaitBestPractices;
using Mopups.Interfaces;

namespace SmartGrowHub.Maui.Mopups;

public sealed partial class LoadingMopup : IDisposable, IAsyncDisposable
{
    private readonly IPopupNavigation _popupNavigation;

    public LoadingMopup(IPopupNavigation popupNavigation)
	{
		InitializeComponent();
        _popupNavigation = popupNavigation;
    }

    public void Dispose() => DisposeAsync().SafeFireAndForget();

    public async ValueTask DisposeAsync() => await _popupNavigation.PopAsync();
}