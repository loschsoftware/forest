using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System;

namespace Losch.LSEdit.Core.UI;

public partial class TemplateButton : UserControl
{
    public TemplateButton()
    {
        InitializeComponent();
    }

    public event EventHandler FavoriteChanged;
    public event RoutedEventHandler Click;

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(FrameworkElement), typeof(TemplateButton), new FrameworkPropertyMetadata(null));
    public static readonly DependencyProperty IconSizeProperty = DependencyProperty.Register("IconSize", typeof(double), typeof(TemplateButton), new FrameworkPropertyMetadata(64.0));
    public static readonly DependencyProperty IconMarginProperty = DependencyProperty.Register("IconMargin", typeof(Thickness), typeof(TemplateButton), new FrameworkPropertyMetadata(new Thickness(15, 5, 5, 5)));
    public static readonly DependencyProperty IconVisibilityProperty = DependencyProperty.Register("IconVisibility", typeof(Visibility), typeof(TemplateButton), new FrameworkPropertyMetadata(null));
    public static readonly DependencyProperty MainTextProperty = DependencyProperty.Register("MainText", typeof(string), typeof(TemplateButton), new FrameworkPropertyMetadata(null));
    public static readonly DependencyProperty TagsProperty = DependencyProperty.Register("Tags", typeof(string), typeof(TemplateButton), new FrameworkPropertyMetadata(null));
    public static readonly DependencyProperty TagsVisibilityProperty = DependencyProperty.Register("TagsVisibility", typeof(Visibility), typeof(TemplateButton), new FrameworkPropertyMetadata(null));
    public static readonly DependencyProperty FavoriteStarVisibilityProperty = DependencyProperty.Register("FavoriteStarVisibility", typeof(Visibility), typeof(TemplateButton), new FrameworkPropertyMetadata(Visibility.Collapsed));
    public static readonly DependencyProperty DescriptionTextProperty = DependencyProperty.Register("DescriptionText", typeof(string), typeof(TemplateButton), new FrameworkPropertyMetadata(null));
    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(TemplateButton), new FrameworkPropertyMetadata(null));
    public static readonly DependencyProperty IsFavoriteProperty = DependencyProperty.Register("IsFavorite", typeof(bool), typeof(TemplateButton), new FrameworkPropertyMetadata(null));

    public ObservableCollection<Border> TagElements { get; set; } = new();

    public FrameworkElement Icon
    {
        get => (FrameworkElement)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public double IconSize
    {
        get => (double)GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }

    public Thickness IconMargin
    {
        get => (Thickness)GetValue(IconMarginProperty);
        set => SetValue(IconMarginProperty, value);
    }

    public Visibility IconVisibility
    {
        get => (Visibility)GetValue(IconVisibilityProperty);
        set => SetValue(IconVisibilityProperty, value);
    }

    public Visibility FavoriteStarVisibility
    {
        get => (Visibility)GetValue(FavoriteStarVisibilityProperty);
        set => SetValue(FavoriteStarVisibilityProperty, value);
    }

    public bool IsFavorite
    {
        get => (bool)GetValue(IsFavoriteProperty);
        set
        {
            SetValue(IsFavoriteProperty, value);
            FavoriteChanged?.Invoke(this, new());
        }
    }

    public string MainText
    {
        get => (string)GetValue(MainTextProperty);
        set => SetValue(MainTextProperty, value);
    }

    public string Tags
    {
        get => (string)GetValue(TagsProperty);
        set
        {
            SetValue(TagsProperty, value ??= "");

            foreach (string str in value.Split(';'))
            {
                TagElements.Add(new Border()
                {
                    CornerRadius = new(3),
                    Margin = new(0, 0, 5, 0),
                    Background = (Brush)Application.Current.FindResource(AdonisUI.Brushes.Layer3BackgroundBrush),
                    BorderBrush = (Brush)Application.Current.FindResource(AdonisUI.Brushes.Layer4BorderBrush),
                    Child = new TextBlock()
                    {
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Text = str,
                        Margin= new(5, 3, 5, 3)
                    }
                });
            }
        }
    }

    public Visibility TagsVisibility
    {
        get => (Visibility)GetValue(TagsVisibilityProperty);
        set => SetValue(TagsVisibilityProperty, value);
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
        MainBorder.Background = (Brush)FindResource(AdonisUI.Brushes.AccentBrush);
    }

    private void UserControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        MainBorder.Background = (Brush)FindResource(AdonisUI.Brushes.AccentBrush);

        Click?.Invoke(sender, e);
        Command?.Execute(null);
    }

    private void UserControl_MouseLeave(object sender, MouseEventArgs e)
    {
        MainBorder.Background = (Brush)FindResource(AdonisUI.Brushes.Layer2BackgroundBrush);
    }

    private void UserControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        MainBorder.Background = (Brush)FindResource(AdonisUI.Brushes.AccentHighlightBrush);
    }
}