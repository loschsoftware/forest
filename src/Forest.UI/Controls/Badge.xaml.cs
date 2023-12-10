using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Forest.UI.Controls;

public partial class Badge : UserControl
{
    public Badge()
    {
        InitializeComponent();
    }

    public static Badge Orange(string text) => new()
    {
        BorderBrush = (Brush)new BrushConverter().ConvertFromString("#FFC108"),
        Foreground = (Brush)new BrushConverter().ConvertFromString("#000000"),
        BadgeBackground = (Brush)new BrushConverter().ConvertFromString("#FFD55A"),
        Text = text,
        Margin = new(0, 0, 5, 0)
    };

    public static Badge Red(string text) => new()
    {
        BorderBrush = (Brush)new BrushConverter().ConvertFromString("#D32F2F"),
        Foreground = (Brush)new BrushConverter().ConvertFromString("#FFFFFF"),
        BadgeBackground = (Brush)new BrushConverter().ConvertFromString("#E17474"),
        Text = text,
        Margin = new(0, 0, 5, 0)
    };

    public static Badge Blue(string text) => new()
    {
        BorderBrush = (Brush)new BrushConverter().ConvertFromString("#4752B9"),
        Foreground = (Brush)new BrushConverter().ConvertFromString("#FFFFFF"),
        BadgeBackground = (Brush)new BrushConverter().ConvertFromString("#848BD0"),
        Text = text,
        Margin = new(0, 0, 5, 0)
    };

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(Badge), new FrameworkPropertyMetadata(null));
    public static readonly DependencyProperty BadgeBackgroundProperty = DependencyProperty.Register("BadgeBackground", typeof(Brush), typeof(Badge), new FrameworkPropertyMetadata(null));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public Brush BadgeBackground
    {
        get => (Brush)GetValue(BadgeBackgroundProperty);
        set => SetValue(BadgeBackgroundProperty, value);
    }
}