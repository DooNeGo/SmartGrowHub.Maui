namespace SmartGrowHub.Maui.Features.ConfigureGrowHub.View;

public sealed partial class LightControlPage
{
    public LightControlPage()
    {
        InitializeComponent();
    }

    private void Slider_OnDragCompleted(object? sender, EventArgs e)
    {
        Slider.Value = Math.Floor(Slider.Value);
    }
}