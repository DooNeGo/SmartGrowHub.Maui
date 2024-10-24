using SmartGrowHub.Maui.Application.Interfaces;

namespace SmartGrowHub.Maui.Popups;

public sealed partial class LoadingPopup
{
    private readonly IDialogService _dialogService;

    public LoadingPopup(IDialogService dialogService)
    {
        InitializeComponent();
        _dialogService = dialogService;
    }

    protected override bool OnBackButtonPressed()
    {
        _dialogService.Pop().Run();
        return true;
    }
}