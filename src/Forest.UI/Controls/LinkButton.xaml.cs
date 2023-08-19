using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Forest.UI.Controls;

public partial class LinkButton : UserControl
{
    public LinkButton()
    {
        InitializeComponent();
    }

    public double MaxTextWidth => Width - IconFrame.Width - 20;

    public event RoutedEventHandler Click;

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(object), typeof(LinkButton), new FrameworkPropertyMetadata(null));
    public static readonly DependencyProperty IconVisibilityProperty = DependencyProperty.Register("IconVisibility", typeof(Visibility), typeof(LinkButton), new FrameworkPropertyMetadata(null));
    public static readonly DependencyProperty MainTextProperty = DependencyProperty.Register("MainText", typeof(string), typeof(LinkButton), new FrameworkPropertyMetadata(null));
    public static readonly DependencyProperty DescriptionTextProperty = DependencyProperty.Register("DescriptionText", typeof(string), typeof(LinkButton), new FrameworkPropertyMetadata(null));
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(LinkButton), new FrameworkPropertyMetadata(null));

    public object Icon
    {
        get => (object)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public Visibility IconVisibility
    {
        get => (Visibility)GetValue(IconVisibilityProperty);
        set => SetValue(IconVisibilityProperty, value);
    }

    public string MainText
    {
        get => (string)GetValue(MainTextProperty);
        set => SetValue(MainTextProperty, value);
    }

    public string DescriptionText
    {
        get => (string)GetValue(DescriptionTextProperty);
        set => SetValue(DescriptionTextProperty, value);
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    private void UserControl_MouseEnter(object sender, MouseEventArgs e)
    {
        MainBorder.Background = (Brush)FindResource(AdonisUI.Brushes.Layer3BackgroundBrush);
        MainBorder.BorderBrush = (Brush)FindResource(AdonisUI.Brushes.Layer3BorderBrush);
    }

    private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        MainBorder.Background = (Brush)FindResource(AdonisUI.Brushes.Layer3BackgroundBrush);
        MainBorder.BorderBrush = (Brush)FindResource(AdonisUI.Brushes.Layer3BorderBrush);

        Click?.Invoke(sender, e);
        Command?.Execute(null);
    }

    private void UserControl_MouseLeave(object sender, MouseEventArgs e)
    {
        MainBorder.Background = (Brush)FindResource(AdonisUI.Brushes.Layer2BackgroundBrush);
        MainBorder.BorderBrush = (Brush)FindResource(AdonisUI.Brushes.Layer2BorderBrush);
    }

    private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        MainBorder.Background = (Brush)FindResource(AdonisUI.Brushes.Layer3BorderBrush);
        MainBorder.BorderBrush = (Brush)FindResource(AdonisUI.Brushes.Layer3BackgroundBrush);
    }
}