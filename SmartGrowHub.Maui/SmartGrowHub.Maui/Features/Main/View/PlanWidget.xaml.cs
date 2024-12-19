using System.Windows.Input;

namespace SmartGrowHub.Maui.Features.Main.View;

public sealed partial class PlanWidget
{
    #region IconSource

    public static readonly BindableProperty IconSourceProperty = BindableProperty.Create(
        nameof(IconSource), typeof(string), typeof(PlanWidget), string.Empty);

    public string IconSource
    {
        get => (string)GetValue(IconSourceProperty);
        set => SetValue(IconSourceProperty, value);
    }

    #endregion
    
    #region PlanName

    public static readonly BindableProperty PlanNameProperty = BindableProperty.Create(
        nameof(PlanName), typeof(string), typeof(PlanWidget), string.Empty);

    public string PlanName
    {
        get => (string)GetValue(PlanNameProperty);
        set => SetValue(PlanNameProperty, value);
    }

    #endregion
    
    #region Command

    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command), typeof(ICommand), typeof(PlanWidget));

    public ICommand? Command
    {
        get => GetValue(CommandProperty) as ICommand;
        set => SetValue(CommandProperty, value);
    }

    #endregion
    
    public PlanWidget()
    {
        InitializeComponent();
    }
}