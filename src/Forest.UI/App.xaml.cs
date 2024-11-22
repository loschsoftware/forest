using Forest.UI;
using Forest.UI.ViewModels;
using Forest.UI.Views;
using Forest.UI.Views.WizardSteps;
using Forest.ViewModels;
using Forest.Views;
using Losch.Installer;
using Losch.Installer.LinFile;
using Losch.Installer.PackageManifest;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Serialization;

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
        MainView main = new();
        MainViewModel vm = main.DataContext as MainViewModel;

        if (!string.IsNullOrEmpty(Program.package))
        {
            string packagesDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Losch", "Installer", "Packages");
            foreach (string dir in Directory.GetDirectories(packagesDir))
            {
                string manifestFile = Path.Combine(dir, "manifest.xml");
                if (File.Exists(manifestFile))
                {
                    try
                    {
                        using StreamReader sr = new(manifestFile);
                        XmlSerializer xmls = new(typeof(PackageManifestFile));
                        PackageManifestFile manifest = (PackageManifestFile)xmls.Deserialize(sr);

                        if (manifest.PackageInfo.UniqueId.Equals(Program.package, StringComparison.OrdinalIgnoreCase))
                        {
                            PackageDetailsPage page = new(manifest);

                            if (Program.disableLaunchButton)
                                (page.DataContext as PackageDetailsViewModel).LaunchButtonVisibility = Visibility.Collapsed;

                            vm.CurrentPage = page;
                            vm.StartPageButtonVisibility = Visibility.Collapsed;

                            break;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        if (Environment.GetCommandLineArgs().Length > 1 && File.Exists(Environment.GetCommandLineArgs()[1]))
        {
            LoadingView loadingView = new();
            loadingView.Show();

            try
            {
                lr = new(Environment.GetCommandLineArgs()[1]);

                vm.NextButtonVisibility = Visibility.Visible;
                vm.BackButtonVisibility = Visibility.Visible;
                vm.StartPageButtonVisibility = Visibility.Collapsed;

                vm.InstallerPages = new()
                {
                    { 0, lr.GetManifest().Steps.ToList() }
                };
            }
            catch (Exception ex)
            {
                AdonisUI.Controls.MessageBox.Show(
                    string.Format((string)Current.TryFindResource("StringInvalidPackageMessage"), ex.ToString()),
                    (string)Current.TryFindResource("StringAppTitle"),
                    AdonisUI.Controls.MessageBoxButton.OK,
                    AdonisUI.Controls.MessageBoxImage.Error);

                Current.Shutdown();
            }

            loadingView.Close();
        }

        main.Show();
    }
}