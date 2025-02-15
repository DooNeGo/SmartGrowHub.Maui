using System.ComponentModel;
using System.Windows.Input;
using CommunityToolkit.Maui.Core;

namespace SmartGrowHub.Maui.Features.Main.View;

public sealed partial class QuickSetting
{
    #region IconSource

    public static readonly BindableProperty IconSourceProperty = BindableProperty.Create(
        nameof(IconSource), typeof(ImageSource), typeof(QuickSetting));

    [TypeConverter(typeof(ImageSourceConverter))]
    public ImageSource? IconSource
    {
        get => GetValue(IconSourceProperty) as ImageSource;
        set => SetValue(IconSourceProperty, value);
    }

    #endregion
    
    #region IconColor

    public static readonly BindableProperty IconColorProperty = BindableProperty.Create(
        nameof(IconColor), typeof(Color), typeof(QuickSetting), new Color());

    public Color IconColor
    {
        get => (Color)GetValue(IconColorProperty);
        set => SetValue(IconColorProperty, value);
    }
    
    #endregion
    
    #region Status

    public static readonly BindableProperty StatusProperty = BindableProperty.Create(
        nameof(Status), typeof(string), typeof(QuickSetting), string.Empty);

    public string Status
    {
        get => (string)GetValue(StatusProperty);
        set => SetValue(StatusProperty, value);
    }

    #endregion
    
    #region Value

    public static readonly BindableProperty ValueProperty = BindableProperty.Create(
        nameof(Value), typeof(string), typeof(QuickSetting), string.Empty);

    public string Value
    {
        get => GetValue(ValueProperty) as string ?? string.Empty;
        set => SetValue(ValueProperty, value);
    }

    #endregion
    
    #region Command

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command), typeof(ICommand), typeof(QuickSetting));

    public ICommand? Command
    {
        get => GetValue(CommandProperty) as ICommand;
        set => SetValue(CommandProperty, value);
    }

    #endregion
    
    #region CommandParameter

    public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(
        nameof(CommandParameter), typeof(object), typeof(QuickSetting));

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    #endregion
    
    public QuickSetting()
    {
        InitializeComponent();
    }

    public event EventHandler<TouchGestureCompletedEventArgs>? Tapped;

    private void TouchBehavior_OnTouchGestureCompleted(object? sender, TouchGestureCompletedEventArgs e) =>
        Tapped?.Invoke(this, e);
}