namespace SmartGrowHub.Maui.Features.Main.View;

public sealed partial class DeviceWidget
{
    #region DeviceName

    public static readonly BindableProperty DeviceNameProperty = BindableProperty.Create(
        nameof(DeviceName), typeof(string), typeof(DeviceWidget), string.Empty);

    public string DeviceName
    {
        get => (string)GetValue(DeviceNameProperty);
        set => SetValue(DeviceNameProperty, value);
    }

    #endregion
    
    #region ModelName

    public static readonly BindableProperty ModelNameProperty = BindableProperty.Create(
        nameof(ModelName), typeof(string), typeof(DeviceWidget), string.Empty);

    public string ModelName
    {
        get => (string)GetValue(ModelNameProperty);
        set => SetValue(ModelNameProperty, value);
    }

    #endregion
    
    #region DeviceIconSource

    public static readonly BindableProperty DeviceIconSourceProperty = BindableProperty.Create(
        nameof(DeviceIconSource), typeof(string), typeof(DeviceWidget), string.Empty);

    public string DeviceIconSource
    {
        get => (string)GetValue(DeviceIconSourceProperty);
        set => SetValue(DeviceIconSourceProperty, value);
    }

    #endregion
    
    #region IsConnected

    public static readonly BindableProperty IsConnectedProperty = BindableProperty.Create(
        nameof(IsConnected), typeof(bool), typeof(DeviceWidget), false);

    public bool IsConnected
    {
        get => (bool)GetValue(IsConnectedProperty);
        set => SetValue(IsConnectedProperty, value);
    }

    #endregion
    
    public DeviceWidget()
    {
        InitializeComponent();
    }
}