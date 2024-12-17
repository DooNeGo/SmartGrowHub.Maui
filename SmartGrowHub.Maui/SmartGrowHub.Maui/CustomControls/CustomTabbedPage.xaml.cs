using System.Collections;

namespace SmartGrowHub.Maui.CustomControls;

public sealed partial class CustomTabbedPage
{
    #region TabsSource

    public static readonly BindableProperty TabsSourceProperty = BindableProperty.Create(
        nameof(TabsSource), typeof(IEnumerable), typeof(CustomTabbedPage), Enumerable.Empty<object>());

    public IEnumerable TabsSource
    {
        get => (IEnumerable)GetValue(TabsSourceProperty);
        set => SetValue(TabsSourceProperty, value);
    }

    #endregion
    
    #region TabTemplate

    public static readonly BindableProperty TabTemplateProperty = BindableProperty.Create(
        nameof(TabTemplate), typeof(DataTemplate), typeof(CustomTabbedPage));

    public DataTemplate? TabTemplate
    {
        get => GetValue(TabTemplateProperty) as DataTemplate;
        set => SetValue(TabTemplateProperty, value);
    }

    #endregion
    
    #region ItemsSource

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
        nameof(ItemsSource), typeof(IEnumerable), typeof(CustomTabbedPage), Enumerable.Empty<object>());

    public IEnumerable ItemsSource
    {
        get => (IEnumerable)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    #endregion
    
    #region ItemTemplate

    public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(
        nameof(ItemTemplate), typeof(DataTemplate), typeof(CustomTabbedPage));

    public DataTemplate? ItemTemplate
    {
        get => GetValue(ItemTemplateProperty) as DataTemplate;
        set => SetValue(ItemTemplateProperty, value);
    }
    
    #endregion
    
    public CustomTabbedPage()
    {
        InitializeComponent();
    }
}