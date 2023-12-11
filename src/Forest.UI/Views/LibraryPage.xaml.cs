using Forest.UI.Controls;
using Forest.UI.ViewModels;
using System.Windows.Controls;

namespace Forest.UI.Views;

public partial class LibraryPage : Page
{
    public LibraryPage()
    {
        InitializeComponent();
        FilterContainer.Content = new LibraryFilter(DataContext as LibraryViewModel);
    }
}