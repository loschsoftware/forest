using CommunityToolkit.Mvvm.ComponentModel;
using Losch.Installer.PackageManifest;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace Forest.UI.ViewModels;

public class PackageDetailsViewModel : ObservableObject
{
    public PackageDetailsViewModel(PackageManifestFile manifest)
    {
        this.manifest = manifest;

        PackageTitle = manifest.PackageInfo.PackageName;
        PackageId = manifest.PackageInfo.UniqueId;
        LaunchButtonVisibility = manifest.CanStart ? Visibility.Visible : Visibility.Collapsed;

        if (File.Exists(manifest.PackageInfo.IconFile))
        {
            Icon = Path.GetExtension(manifest.PackageInfo.IconFile) switch
            {
                ".xaml" => new Image()
                {
                    Source = (DrawingImage)XamlReader.Parse(File.ReadAllText(manifest.PackageInfo.IconFile)),
                },
                _ => new Image()
                {
                    Source = new BitmapImage(new(manifest.PackageInfo.IconFile))
                }
            };
        }

        Developer = manifest.PackageInfo.Author;
        Publisher = manifest.PackageInfo.Publisher;
        Version = manifest.Version;
        InstallationDate = manifest.InstallationDate;
        Location = manifest.BaseDirectory;
    }

    private PackageManifestFile manifest;

    private Visibility _launchButtonVisibility = Visibility.Visible;
    public Visibility LaunchButtonVisibility
    {
        get => _launchButtonVisibility;
        set => SetProperty(ref _launchButtonVisibility, value);
    }

    private string _packageTitle;
    public string PackageTitle
    {
        get => _packageTitle;
        set => SetProperty(ref _packageTitle, value);
    }

    private string _packageId;
    public string PackageId
    {
        get => _packageId;
        set => SetProperty(ref _packageId, value);
    }

    private FrameworkElement _icon;
    public FrameworkElement Icon
    {
        get => _icon;
        set => SetProperty(ref _icon, value);
    }

    private string _developer;
    public string Developer
    {
        get => _developer;
        set => SetProperty(ref _developer, value);
    }

    private string _publisher;
    public string Publisher
    {
        get => _publisher;
        set => SetProperty(ref _publisher, value);
    }

    private string _version;
    public string Version
    {
        get => _version;
        set => SetProperty(ref _version, value);
    }

    private string _installationDate;
    public string InstallationDate
    {
        get => _installationDate;
        set => SetProperty(ref _installationDate, value);
    }

    private string _location;
    public string Location
    {
        get => _location;
        set => SetProperty(ref _location, value);
    }

    public ICommand LaunchCommand => new RelayCommand(() =>
    {
        Process.Start(manifest.StartupAction);
    });
}