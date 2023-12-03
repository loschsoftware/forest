using Forest.ViewModels;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
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

    private void InstallPackage(object sender, RoutedEventArgs e)
    {
        OpenFileDialog ofd = new()
        {
            Multiselect = false,
            Filter = (string)Application.Current.TryFindResource("StringLinFileFilter")
        };

        if (ofd.ShowDialog() == true)
        {
            if (Path.GetExtension(ofd.FileName).Equals(".exe", System.StringComparison.OrdinalIgnoreCase))
            {
                Process.Start(ofd.FileName);
                Application.Current.Shutdown();
            }

            Process.Start("linui.exe", ofd.FileName);
            Application.Current.Shutdown();
        }
    }

    private void ShowLibrary(object sender, RoutedEventArgs e)
    {
        vm.ShowLibraryCommand.Execute(null);
    }
}