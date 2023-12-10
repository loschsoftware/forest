using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Forest.UI.Controls;
using Losch.Installer.PackageManifest;
using Losch.LSEdit.Core.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace Forest.UI.ViewModels;

public class LibraryViewModel : ObservableObject
{
    public LibraryViewModel()
    {
        filterPopup = new()
        {
            IsOpen = false,
            Content = new LibraryFilter(this),
            Placement = PlacementMode.Mouse
        };

        Badges = [Badge.Orange("Update verfügbar"), Badge.Red("Veraltet"), Badge.Blue((string)Application.Current.TryFindResource("StringNew")), Badge.Red((string)Application.Current.TryFindResource("StringUnverified"))];

        ObservableCollection<FrameworkElement> buttons = new();

        string packagesDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Losch", "Installer", "Packages");
        if (!Directory.Exists(packagesDir))
            Directory.CreateDirectory(packagesDir);

        foreach (string package in Directory.EnumerateDirectories(packagesDir))
        {
            bool fail = false;
            PackageManifestFile manifest = null;

            if (!File.Exists(Path.Combine(package, "manifest.xml")))
                fail = true;
            else
            {
                try
                {
                    using StreamReader sr = new(Path.Combine(package, "manifest.xml"));
                    XmlSerializer xmls = new(typeof(PackageManifestFile));
                    manifest = (PackageManifestFile)xmls.Deserialize(sr);
                }
                catch (Exception)
                {
                    fail = true;
                }
            }

            if (fail)
            {
                buttons.Add(new TemplateButton()
                {
                    MainText = string.Format((string)Application.Current.TryFindResource("StringUnknownPackage"), package.Split("\\").Last()),
                    DescriptionText = (string)Application.Current.TryFindResource("StringUnknownPackageDescription"),
                    Icon = new Viewbox()
                    {
                        Height = 48,
                        Width = 48,
                        Child = (Viewbox)Application.Current.TryFindResource("F1Help")
                    },
                    BadgesVisibility = Visibility.Collapsed,
                    TagsVisibility = Visibility.Collapsed,
                    BorderThickness = new(0, 0, 0, 1),
                    BorderBrush = (Brush)Application.Current.FindResource(AdonisUI.Brushes.Layer1BorderBrush)
                });

                continue;
            }

            List<Badge> badges = [];

            if (-(Directory.GetCreationTime(package) - DateTime.Now).TotalDays < 3)
                badges.Add(Badge.Blue((string)Application.Current.TryFindResource("StringNew")));

            string publisher = manifest.PackageInfo.Publisher, version = manifest.PackageInfo.Version;

            FrameworkElement icon = new Viewbox()
            {
                Height = 48,
                Width = 48,
                Child = (Viewbox)Application.Current.TryFindResource("F1Help")
            };

            if (File.Exists(manifest.PackageInfo.IconFile))
            {
                icon = Path.GetExtension(manifest.PackageInfo.IconFile) switch
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

            if (manifest.PackageInfo.Publisher.ToLowerInvariant() != "losch")
                badges.Add(Badge.Red((string)Application.Current.TryFindResource("StringUnverified")));

            TemplateButton button = new()
            {
                MainText = string.Format((string)Application.Current.TryFindResource("StringPackageTitleFormat"), manifest.PackageInfo.PackageName, manifest.PackageInfo.UniqueId),
                DescriptionText = $"{(string)Application.Current.TryFindResource("StringPublisher")}: {publisher ?? (string)Application.Current.TryFindResource("StringUnknown")}\r\n{(string)Application.Current.TryFindResource("StringVersion")}: {version ?? (string)Application.Current.TryFindResource("StringUnknown")}",
                Icon = icon,
                TagsVisibility = Visibility.Collapsed,
                Badges = badges,
                BadgesVisibility = badges.Any() ? Visibility.Visible : Visibility.Collapsed,
                BorderThickness = new(0, 0, 0, 1),
                BorderBrush = (Brush)Application.Current.FindResource(AdonisUI.Brushes.Layer1BorderBrush)
            };

            buttons.Add(button);
        }

        if (buttons.Count == 0)
        {
            buttons.Add(new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Text = (string)Application.Current.TryFindResource("StringNoPackagesFound")
            });
        }

        Packages = buttons;
        _allPackages = buttons;
    }

    private ObservableCollection<FrameworkElement> _allPackages;

    private string _searchQuery;
    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            SetProperty(ref _searchQuery, value);

            if (!_allPackages.Any(p => p is TemplateButton))
                return;

            Packages = _allPackages.Where(t =>
                (t as TemplateButton).MainText.Contains(value, StringComparison.OrdinalIgnoreCase)
                || (t as TemplateButton).DescriptionText.Contains(value, StringComparison.OrdinalIgnoreCase));

            if (!Packages.Any())
            {
                Packages = Packages.Append(new TextBlock()
                {
                    Name = "NoPackagesFoundTextBlock",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Text = (string)Application.Current.TryFindResource("StringSearchQueryNoPackagesFound")
                });
            }
        }
    }

    private IEnumerable<FrameworkElement> _packages = [];
    public IEnumerable<FrameworkElement> Packages
    {
        get => _packages;
        set => SetProperty(ref _packages, value);
    }

    private BalloonPopup filterPopup = null;

    private bool _filterPopupOpen = false;
    public bool FilterPopupOpen
    {
        get => _filterPopupOpen;
        set
        {
            SetProperty(ref _filterPopupOpen, value);
            filterPopup.IsOpen = value;
        }
    }

    private ObservableCollection<Badge> _badges = [];
    public ObservableCollection<Badge> Badges
    {
        get => _badges;
        set => SetProperty(ref _badges, value);
    }

    public ICommand FocusCommand => new RelayCommand<FrameworkElement>(f =>
    {
        f.Focus();
    });
}