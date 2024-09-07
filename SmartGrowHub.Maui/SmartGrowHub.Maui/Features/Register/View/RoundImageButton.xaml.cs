namespace SmartGrowHub.Maui.Features.Register.View;

public sealed partial class RoundImageButton
{
    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
        nameof(ImageSource), typeof(string), typeof(RoundImageButton), string.Empty,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is not RoundImageButton button) return;
            if (newValue is not string imagePath) return;
            button.Image.Source = imagePath;
        });


    public RoundImageButton() => InitializeComponent();

    public string ImageSource
    {
        get => (string)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }
}