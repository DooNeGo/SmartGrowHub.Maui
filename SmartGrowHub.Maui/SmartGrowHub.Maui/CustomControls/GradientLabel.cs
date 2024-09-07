using Microsoft.Maui.Graphics.Text;
using Font = Microsoft.Maui.Graphics.Font;

namespace SmartGrowHub.Maui.CustomControls;

public sealed class GradientLabel : GraphicsView
{
    private const float DefaultFontSize = 14;

    public static readonly BindableProperty TextProperty = BindableProperty.Create(
        nameof(Text), typeof(string), typeof(GradientLabel), string.Empty, propertyChanged: OnTextChanged);

    public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(
        nameof(FontSize), typeof(float), typeof(GradientLabel), DefaultFontSize, propertyChanged: OnTextChanged);

    public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(
        nameof(FontFamily), typeof(string), typeof(GradientLabel), string.Empty, propertyChanged: OnTextChanged);

    public static readonly BindableProperty FontWeightProperty = BindableProperty.Create(
        nameof(FontWeight), typeof(int), typeof(GradientLabel), FontWeights.Normal, propertyChanged: OnTextChanged);

    public static readonly BindableProperty BrushProperty = BindableProperty.Create(
        nameof(Brush), typeof(Brush), typeof(GradientLabel), Brush.Default, propertyChanged: OnTextChanged);

    public GradientLabel()
    {
        Drawable = new TextDrawable(this);
        Invalidate();
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public float FontSize
    {
        get => (float)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public string FontFamily
    {
        get => (string)GetValue(FontFamilyProperty);
        set => SetValue(FontFamilyProperty, value);
    }

    public int FontWeight
    {
        get => (int)GetValue(FontWeightProperty);
        set => SetValue(FontWeightProperty, value);
    }

    public Brush Brush
    {
        get => (Brush)GetValue(BrushProperty);
        set => SetValue(BrushProperty, value);
    }

    private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var label = (GradientLabel)bindable;
        label.Invalidate();
    }

    private sealed class TextDrawable(GradientLabel label1) : IDrawable
    {
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            GradientLabel label = label1;

            //if (string.IsNullOrEmpty(label.Text)) return;

            var stringSize = canvas.GetStringSize("HELLO, WORLD!", Font.Default, 20);

            var stringBounds = new RectF(dirtyRect.Location, stringSize);

            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 2;
            canvas.DrawRectangle(stringBounds);

            canvas.FontSize = 20;

            canvas.DrawString("HELLO, WORLD!", stringBounds, HorizontalAlignment.Left, VerticalAlignment.Top);

            ////canvas.SetFillPaint(label.Brush, dirtyRect);
            //canvas.FillColor = Colors.Black;
            //canvas.FontSize = label.FontSize;
            ////canvas.Font = new Font(label.FontFamily, label.FontWeight);
            //canvas.DrawString("Text", 0, 0, dirtyRect.Width, dirtyRect.Height, HorizontalAlignment.Left, VerticalAlignment.Top, textFlow: TextFlow.OverflowBounds);
            ////canvas.DrawString(label.Text, dirtyRect.Center.X, dirtyRect.Center.Y, HorizontalAlignment.Center);
        }
    }
}