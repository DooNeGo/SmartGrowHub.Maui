namespace SmartGrowHub.Maui.CustomControls;

public sealed partial class CustomEntry
{
    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text), typeof(string), typeof(CustomEntry), string.Empty,
        propertyChanged: (bindable, _, newValue) =>
        {
            if (bindable is not CustomEntry entry) return;
            if (newValue is not string value) return;
            entry.Entry.Text = value;
        });

    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
        nameof(Placeholder), typeof(string), typeof(CustomEntry), string.Empty,
        propertyChanged: (bindable, _, newValue) =>
        {
            if (bindable is not CustomEntry entry) return;
            if (newValue is not string value) return;
            entry.Entry.Placeholder = value;
        });

    public static readonly BindableProperty IconSourceProperty = BindableProperty.Create(
        nameof(IconSource), typeof(string), typeof(CustomEntry), string.Empty,
        propertyChanged: (bindable, _, newValue) =>
        {
            if (bindable is not CustomEntry entry) return;
            if (newValue is not string value) return;
            entry.Image.Source = value;
        });

    public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(
        nameof(IsPassword), typeof(bool), typeof(CustomEntry), false,
        propertyChanged: (bindable, _, newValue) =>
        {
            if (bindable is not CustomEntry entry) return;
            if (newValue is not bool value) return;
            entry.Entry.IsPassword = value;
        });

    public CustomEntry() => InitializeComponent();

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public string IconSource
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }
}