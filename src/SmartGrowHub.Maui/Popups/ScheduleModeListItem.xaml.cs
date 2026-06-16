namespace SmartGrowHub.Maui.Popups;

public sealed partial class ScheduleModeListItem
{
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(ScheduleModeListItem));
    
    public static readonly BindableProperty DescriptionProperty =
        BindableProperty.Create(nameof(Description), typeof(string), typeof(ScheduleModeListItem));
    
    public ScheduleModeListItem()
    {
        InitializeComponent();
    }
    
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    
    public string Description
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }
}
