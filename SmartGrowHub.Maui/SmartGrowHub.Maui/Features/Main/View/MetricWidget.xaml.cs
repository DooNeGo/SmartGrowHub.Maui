namespace SmartGrowHub.Maui.Features.Main.View;

public sealed partial class MetricWidget
{
    #region MainValue

    public static readonly BindableProperty MainValueProperty = BindableProperty.Create(
        nameof(MainValue), typeof(string), typeof(MetricWidget), string.Empty);

    public string MainValue
    {
        get => (string)GetValue(MainValueProperty);
        set => SetValue(MainValueProperty, value);
    }
    
    #endregion
    
    #region ValueUnit

    public static readonly BindableProperty ValueUnitProperty = BindableProperty.Create(
        nameof(ValueUnit), typeof(string), typeof(MetricWidget), string.Empty);

    public string ValueUnit
    {
        get => (string)GetValue(ValueUnitProperty);
        set => SetValue(ValueUnitProperty, value);
    }
    
    #endregion
    
    #region MetricName

    public static readonly BindableProperty MetricNameProperty = BindableProperty.Create(
        nameof(MetricName), typeof(string), typeof(MetricWidget), string.Empty);

    public string MetricName
    {
        get => (string)GetValue(MetricNameProperty);
        set => SetValue(MetricNameProperty, value);
    }
    
    #endregion
    
    public MetricWidget()
    {
        InitializeComponent();
    }
}