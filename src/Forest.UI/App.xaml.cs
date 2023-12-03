using Forest.UI.Views;
using Forest.UI.Views.WizardSteps;
using Forest.ViewModels;
using Forest.Views;
using Losch.Installer;
using Losch.Installer.LinFile;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Forest;

public partial class App : Application
{
    internal static LinReader lr;

    public static Page GetPage(string id) => id switch
    {
        "WelcomePage" => new WelcomePage(),
        "FinishPage" => new FinishPage(),
        "" or null => new InstallationProgressPage(),
        _ => ResourceProvider.GetCustomPage(id)
    };

    private void Application_Startup(object sender, StartupEventArgs e)
    {
        LoadingView loadingView = new();
        loadingView.Show();

        MainView main = new();
        MainViewModel vm = main.DataContext as MainViewModel;

        if (Environment.GetCommandLineArgs().Length > 1 && File.Exists(Environment.GetCommandLineArgs()[1]))
        {
            try
            {
                lr = new(Environment.GetCommandLineArgs()[1]);

                vm.NextButtonVisibility = Visibility.Visible;
                vm.BackButtonVisibility = Visibility.Visible;

                vm.InstallerPages = new()
                {
                    { 0, lr.GetManifest().Steps.ToList() }
                };

            }
            catch (Exception)
            {
                AdonisUI.Controls.MessageBox.Show(
                    (string)Current.TryFindResource("StringInvalidPackageMessage"),
                    (string)Current.TryFindResource("StringAppTitle"),
                    AdonisUI.Controls.MessageBoxButton.OK,
                    AdonisUI.Controls.MessageBoxImage.Error);

                Current.Shutdown();
            }
        }

        main.Show();
        loadingView.Close();
    }
}