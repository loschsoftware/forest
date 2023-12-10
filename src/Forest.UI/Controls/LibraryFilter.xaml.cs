using Forest.UI.ViewModels;
using System.Windows.Controls;

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
}