using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;

namespace SmartGrowHub.Maui.Behaviors;

[SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
public static class StatusBar
{
    #region StatusBarColor

    public static readonly BindableProperty StatusBarColorProperty = BindableProperty.CreateAttached(
        "StatusBarColor", typeof(Color), typeof(StatusBar), null, propertyChanged: OnStatusBarColorChanged);

    private static void OnStatusBarColorChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if ((bindable, newValue) is not (Page page, Color color)) return;
        page.GetOrAddBehavior<StatusBarBehavior>().StatusBarColor = color;  
    }

    public static Color? GetStatusBarColor(Page page) => page.GetValue(StatusBarColorProperty) as Color;

    public static void SetStatusBarColor(Page page, Color? value) => page.SetValue(StatusBarColorProperty, value);
    
    #endregion
    
    #region StatusBarStyle

    public static readonly BindableProperty StatusBarStyleProperty = BindableProperty.CreateAttached(
        "StatusBarStyle", typeof(StatusBarStyle), typeof(StatusBar), StatusBarStyle.Default,
        propertyChanged: OnStatusBarStyleChanged);

    private static void OnStatusBarStyleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if ((bindable, newValue) is not (Page page, StatusBarStyle style)) return;
        page.GetOrAddBehavior<StatusBarBehavior>().StatusBarStyle = style;
    }

    public static StatusBarStyle GetStatusBarStyle(Page page) =>
        (StatusBarStyle)page.GetValue(StatusBarStyleProperty);

    public static void SetStatusBarStyle(Page page, StatusBarStyle value) =>
        page.SetValue(StatusBarStyleProperty, value);
    
    #endregion
    
    #region StatusBarApplyOn

    public static readonly BindableProperty StatusBarApplyOnProperty = BindableProperty.CreateAttached(
        "StatusBarApplyOn", typeof(StatusBarApplyOn), typeof(StatusBar), default(StatusBarApplyOn),
        propertyChanged: OnStatusBarApplyOnChanged);

    private static void OnStatusBarApplyOnChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if ((bindable, newValue) is not (Page page, StatusBarApplyOn applyOn)) return;
        page.GetOrAddBehavior<StatusBarBehavior>().ApplyOn = applyOn;
    }

    public static StatusBarApplyOn GetStatusBarApplyOn(Page page) =>
        (StatusBarApplyOn)page.GetValue(StatusBarApplyOnProperty);

    public static void SetStatusBarApplyOn(Page page, StatusBarApplyOn value) =>
        page.SetValue(StatusBarApplyOnProperty, value);
    
    #endregion
}