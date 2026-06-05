namespace SmartGrowHub.Maui.CustomControls;

public sealed partial class CustomEntry
{
    #region Text

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text), typeof(string), typeof(CustomEntry), string.Empty, BindingMode.TwoWay);
    
    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    #endregion

    #region Placeholder

    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(
        nameof(Placeholder), typeof(string), typeof(CustomEntry), string.Empty);
    
    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    #endregion

    #region IconSource

    public static readonly BindableProperty IconSourceProperty = BindableProperty.Create(
        nameof(IconSource), typeof(string), typeof(CustomEntry), string.Empty);
    
    public string IconSource
    {
        get => (string)GetValue(IconSourceProperty);
        set => SetValue(IconSourceProperty, value);
    }

    #endregion

    #region IsPassword

    public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create(
        nameof(IsPassword), typeof(bool), typeof(CustomEntry), false);
    
    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }

    #endregion

    #region Keyboard

    public static readonly BindableProperty KeyboardProperty = BindableProperty.Create(
        nameof(Keyboard), typeof(Keyboard), typeof(CustomEntry), Keyboard.Default);
    
    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
    }

    #endregion

    #region HasError

    public static readonly BindableProperty HasErrorProperty = BindableProperty.Create(
        nameof(HasError), typeof(bool), typeof(CustomEntry), false);

    public bool HasError
    {
        get => (bool)GetValue(HasErrorProperty);
        set => SetValue(HasErrorProperty, value);
    }

    #endregion

    #region Error

    public static readonly BindableProperty ErrorProperty = BindableProperty.Create(
        nameof(Error), typeof(string), typeof(CustomEntry), string.Empty,
        propertyChanged: (bindable, _, newValue) =>
        {
            if (bindable is not CustomEntry entry) return;
            if (newValue is not string newText) return;
            entry.HasError = !string.IsNullOrEmpty(newText);
        });

    public string Error
    {
        get => (string)GetValue(ErrorProperty);
        set => SetValue(ErrorProperty, value);
    }
    
    #endregion
    
    #region ReturnType

    public static readonly BindableProperty ReturnTypeProperty = BindableProperty.Create(
        nameof(ReturnType), typeof(ReturnType), typeof(CustomEntry), Microsoft.Maui.ReturnType.Default);

    public ReturnType ReturnType
    {
        get => (ReturnType)GetValue(ReturnTypeProperty);
        set => SetValue(ReturnTypeProperty, value);
    }
    
    #endregion

    #region Behaviors

    public new IList<Behavior> Behaviors => Entry.Behaviors;

    #endregion

    public CustomEntry() => InitializeComponent();
    
    public event EventHandler<TextChangedEventArgs>? TextChanged;

    private void Entry_OnTextChanged(object? sender, TextChangedEventArgs e) => TextChanged?.Invoke(this, e);
}