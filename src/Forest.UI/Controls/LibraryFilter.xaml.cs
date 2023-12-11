using Forest.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Forest.UI.Controls;

public partial class LibraryFilter : UserControl
{
    public LibraryFilter(LibraryViewModel vm)
    {
        InitializeComponent();
        this.vm = vm;
        DataContext = vm;
    }

    private readonly LibraryViewModel vm;

    private void ToggleButton_Checked(object sender, RoutedEventArgs e)
    {
        (sender as ToggleButton).Content = Application.Current.TryFindResource("SortDescending");
        (sender as ToggleButton).ToolTip = Application.Current.TryFindResource("StringSortDescending");
    }

    private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
    {
        (sender as ToggleButton).Content = Application.Current.TryFindResource("SortAscending");
        (sender as ToggleButton).ToolTip = Application.Current.TryFindResource("StringSortAscending");
    }
}