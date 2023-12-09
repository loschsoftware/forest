using Forest.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Forest.UI.Views;

public partial class AboutPage : Page
{
    public AboutPage()
    {
        InitializeComponent();

        DataContext = Application.Current.MainWindow.DataContext as MainViewModel;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        (DataContext as MainViewModel).NavigateToLastPage.Execute(null);
    }
}
