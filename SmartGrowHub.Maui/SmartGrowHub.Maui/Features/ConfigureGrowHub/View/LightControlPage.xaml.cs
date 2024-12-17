using SmartGrowHub.Maui.Features.ConfigureGrowHub.ViewModel;

namespace SmartGrowHub.Maui.Features.ConfigureGrowHub.View;

public sealed partial class LightControlPage
{
    public LightControlPage(LightControlPageModel pageModel)
    {
        InitializeComponent();
        BindingContext = pageModel;
    }

    private void Slider_OnDragCompleted(object? sender, EventArgs e)
    {
        Slider.Value = Math.Floor(Slider.Value);
    }
}