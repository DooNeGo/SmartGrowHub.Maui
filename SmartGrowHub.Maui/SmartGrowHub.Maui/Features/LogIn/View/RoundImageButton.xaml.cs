namespace SmartGrowHub.Maui.Features.LogIn.View;

public sealed partial class RoundImageButton
{
    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
        nameof(ImageSource), typeof(string), typeof(RoundImageButton), string.Empty);
    
    public RoundImageButton() => InitializeComponent();

    public string ImageSource
    {
        get => (string)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }
}