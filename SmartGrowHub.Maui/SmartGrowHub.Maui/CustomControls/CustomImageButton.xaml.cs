namespace SmartGrowHub.Maui.CustomControls;

public sealed partial class CustomImageButton
{
    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
        nameof(ImageSource), typeof(string), typeof(CustomImageButton), string.Empty,
        propertyChanged: (bindable, oldValue, newValue) =>
        {
            if (bindable is not CustomImageButton button || newValue is not string imagePath) return;
            button.Image.Source = imagePath;
        });

    public CustomImageButton() => InitializeComponent();

    public string ImageSource
    {
        get => (string)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }
}