using System.ComponentModel;

namespace SmartGrowHub.Maui.CustomControls;

public abstract partial class StatefulContentPage
{
    #region LoadingView

    public static readonly BindableProperty LoadingViewProperty = BindableProperty.Create(
        nameof(LoadingView), typeof(View), typeof(StatefulContentPage));

    public View? LoadingView
    {
        get => GetValue(LoadingViewProperty) as View;
        set => SetValue(LoadingViewProperty, value);
    }

    #endregion
    
    #region DefaultView

    public static readonly BindableProperty DefaultViewProperty = BindableProperty.Create(
        nameof(DefaultView), typeof(View), typeof(StatefulContentPage));

    public View? DefaultView
    {
        get => GetValue(DefaultViewProperty) as View;
        set => SetValue(DefaultViewProperty, value);
    }

    #endregion
    
    #region CurrentState

    public static readonly BindableProperty CurrentStateProperty = BindableProperty.Create(
        nameof(CurrentState), typeof(string), typeof(StatefulContentPage), string.Empty);

    public string CurrentState
    {
        get => (string)GetValue(CurrentStateProperty);
        set => SetValue(CurrentStateProperty, value);
    }

    #endregion
    
    public StatefulContentPage(INotifyPropertyChanged pageModel) : base(pageModel) 
    {
        InitializeComponent();
    }
}