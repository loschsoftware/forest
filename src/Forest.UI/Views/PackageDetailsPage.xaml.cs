using Forest.UI.ViewModels;
using Losch.Installer.PackageManifest;
using System.Windows.Controls;

namespace Forest.UI.Views;

public partial class PackageDetailsPage : Page
{
    public PackageDetailsPage(PackageManifestFile manifest)
    {
        InitializeComponent();
        DataContext = new PackageDetailsViewModel(manifest);
    }
}