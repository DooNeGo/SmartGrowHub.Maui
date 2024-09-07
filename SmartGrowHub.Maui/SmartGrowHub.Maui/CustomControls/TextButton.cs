using System.Windows.Input;

namespace SmartGrowHub.Maui.CustomControls;

public sealed class TextButton : Label
{
    public static readonly BindableProperty CommandProperty = BindableProperty.Create(
        nameof(Command), typeof(ICommand), typeof(TextButton),
        propertyChanged: (bindable, _, newValue) =>
        {
            if (bindable is not TextButton button) return;
            if (newValue is not ICommand command) return;
            button._gestureRecognizer.Command = command;
        });

    private readonly TapGestureRecognizer _gestureRecognizer;

    public TextButton()
    {
        _gestureRecognizer = new TapGestureRecognizer();
        _gestureRecognizer.Tapped += (sender, args) => Tapped?.Invoke(sender, args);
        GestureRecognizers.Add(_gestureRecognizer);
    }

    public event EventHandler<TappedEventArgs>? Tapped;

    public ICommand? Command
    {
        get => GetValue(CommandProperty) as ICommand;
        set => SetValue(CommandProperty, value);
    }
}
