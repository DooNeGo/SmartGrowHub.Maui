using System.Diagnostics.CodeAnalysis;
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;

namespace SmartGrowHub.Maui.Behaviors;

[SuppressMessage("Interoperability", "CA1416:Проверка совместимости платформы")]
public static class StatusBar
{
    #region Color

    public static readonly BindableProperty ColorProperty = BindableProperty.CreateAttached(
        "Color", typeof(Color), typeof(StatusBar), Colors.Transparent,
        propertyChanged: (bindable, _, newValue) =>
        {
            if (bindable is not Page page) return;
            if (newValue is not Color color) return;

            StatusBarBehavior behavior = GetOrCreateBehavior(page);
            behavior.StatusBarColor = color;
        });
    
    public static Color GetColor(BindableObject bindable) => (Color)bindable.GetValue(ColorProperty);
    
    public static void SetColor(BindableObject bindable, Color color) => bindable.SetValue(ColorProperty, color);

    #endregion

    #region Style

    public static readonly BindableProperty StyleProperty = BindableProperty.CreateAttached(
        "Style", typeof(StatusBarStyle), typeof(StatusBar), StatusBarStyle.Default,
        propertyChanged: (bindable, _, newValue) =>
        {
            if (bindable is not Page page) return;
            if (newValue is not StatusBarStyle style) return;

            StatusBarBehavior behavior = GetOrCreateBehavior(page);
            behavior.StatusBarStyle = style;
        });

    public static StatusBarStyle GetStyle(BindableObject bindable) => (StatusBarStyle)bindable.GetValue(StyleProperty);

    public static void SetStyle(BindableObject bindable, StatusBarStyle style) =>
        bindable.SetValue(StyleProperty, style);

    #endregion
    
    #region ApplyOn

    public static readonly BindableProperty ApplyOnProperty = BindableProperty.CreateAttached(
        "ApplyOn", typeof(StatusBarApplyOn), typeof(StatusBar), StatusBarApplyOn.OnBehaviorAttachedTo,
        propertyChanged: (bindable, _, newValue) =>
        {
            if (bindable is not Page page) return;
            if (newValue is not StatusBarApplyOn applyOn) return;

            StatusBarBehavior behavior = GetOrCreateBehavior(page);
            behavior.ApplyOn = applyOn;
        });
    
    public static StatusBarApplyOn GetApplyOn(BindableObject bindable) =>
        (StatusBarApplyOn)bindable.GetValue(ApplyOnProperty);
    
    public static void SetApplyOn(BindableObject bindable, StatusBarApplyOn style) =>
        bindable.SetValue(ApplyOnProperty, style);

    #endregion
    
    private static StatusBarBehavior GetOrCreateBehavior(Page page)
    {
        var behavior = (StatusBarBehavior?)page.Behaviors.FirstOrDefault(b => b is StatusBarBehavior);
        if (behavior is not null) return behavior;
        
        behavior = new StatusBarBehavior();
        page.Behaviors.Add(behavior);

        return behavior;
    }
}