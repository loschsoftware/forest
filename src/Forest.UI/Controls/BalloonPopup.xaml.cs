using System.Windows;
using System.Windows.Controls.Primitives;

namespace Forest.UI.Controls;

public partial class BalloonPopup : Popup
{
    public BalloonPopup()
    {
        InitializeComponent();
    }

    public static readonly DependencyProperty ContentProperty = DependencyProperty.Register("Content", typeof(object), typeof(BalloonPopup), new FrameworkPropertyMetadata(null));

    public object Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }
}