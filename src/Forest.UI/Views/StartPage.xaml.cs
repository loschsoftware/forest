using Forest.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Forest.UI.Views;

public partial class StartPage : Page
{
    readonly MainViewModel vm;

    public StartPage(MainViewModel vm)
    {
        InitializeComponent();

        this.vm = vm;
    }

    private void InstallApplication(object sender, RoutedEventArgs e)
    {
        vm.ShowInstallApplicationPageCommand.Execute(null);
    }

    private void ShowLibrary(object sender, RoutedEventArgs e)
    {
        vm.ShowLibraryCommand.Execute(null);
    }
}