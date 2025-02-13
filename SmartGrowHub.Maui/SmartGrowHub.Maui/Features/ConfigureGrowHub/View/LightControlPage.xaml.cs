using SmartGrowHub.Maui.Features.ConfigureGrowHub.ViewModel;

namespace SmartGrowHub.Maui.Features.ConfigureGrowHub.View;

public sealed partial class LightControlPage
{
    public LightControlPage(LightControlPageModel pageModel) : base(pageModel)
    {
        InitializeComponent();
    }

    private void Slider_OnDragCompleted(object? sender, EventArgs e)
    {
        Slider.Value = Math.Floor(Slider.Value);
    }
}