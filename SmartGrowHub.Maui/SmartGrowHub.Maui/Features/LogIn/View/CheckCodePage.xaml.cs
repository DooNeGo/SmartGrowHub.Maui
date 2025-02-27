using SmartGrowHub.Maui.Features.LogIn.ViewModel;

namespace SmartGrowHub.Maui.Features.LogIn.View;

public sealed partial class CheckCodePage
{
    public CheckCodePage(CheckCodePageModel pageModel) : base(pageModel)
    {
        InitializeComponent();
    }

    private void CustomEntry_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (e.NewTextValue.Length is 0) return;
        if (e.NewTextValue[^1] is < '0' or > '9') Entry.Text = e.OldTextValue;
    }
}