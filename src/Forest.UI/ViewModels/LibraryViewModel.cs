using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Forest.UI.Controls;
using Forest.UI.Views;
using Forest.ViewModels;
using Losch.Installer.PackageManifest;
using Losch.LSEdit.Core.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace Forest.UI.ViewModels;

public class LibraryViewModel : ObservableObject
{
    private const double DisabledBadgeOpacity = 0.4;

    public LibraryViewModel()
    {
        Badge updateAvailableBadge = Badge.Orange((string)Application.Current.TryFindResource("StringUpdateAvailable"));
        updateAvailableBadge.Margin = new(0, 0, 5, 5);
        bool updateAvailableBadgeEnabled = true;
        updateAvailableBadge.MouseLeftButtonUp += (_, _) =>
        {
            updateAvailableBadgeEnabled = !updateAvailableBadgeEnabled;
            updateAvailableBadge.Opacity = updateAvailableBadgeEnabled ? 1 : DisabledBadgeOpacity;
        };

        Badge deprecatedBadge = Badge.Red((string)Application.Current.TryFindResource("StringDeprecated"));
        deprecatedBadge.Margin = new(0, 0, 5, 5);
        bool deprecatedBadgeEnabled = true;
        deprecatedBadge.MouseLeftButtonUp += (_, _) =>
        {
            deprecatedBadgeEnabled = !deprecatedBadgeEnabled;
            deprecatedBadge.Opacity = deprecatedBadgeEnabled ? 1 : DisabledBadgeOpacity;
        };

        Badge newBadge = Badge.Blue((string)Application.Current.TryFindResource("StringNew"));
        newBadge.Margin = new(0, 0, 5, 5);
        bool newBadgeEnabled = true;
        newBadge.MouseLeftButtonUp += (_, _) =>
        {
            newBadgeEnabled = !newBadgeEnabled;
            newBadge.Opacity = newBadgeEnabled ? 1 : DisabledBadgeOpacity;
        };

        Badge unverifiedBadge = Badge.Red((string)Application.Current.TryFindResource("StringUnverified"));
        unverifiedBadge.Margin = new(0, 0, 5, 5);
        bool unverifiedBadgeEnabled = true;
        unverifiedBadge.MouseLeftButtonUp += (_, _) =>
        {
            unverifiedBadgeEnabled = !unverifiedBadgeEnabled;
            unverifiedBadge.Opacity = unverifiedBadgeEnabled ? 1 : DisabledBadgeOpacity;
        };

        Badges = [updateAvailableBadge, deprecatedBadge, newBadge, unverifiedBadge];

        ObservableCollection<FrameworkElement> buttons = [];

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

            string publisher = manifest.PackageInfo.Publisher, version = manifest.Version;

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

            button.Click += (s, e) =>
            {
                MainViewModel mvm = Application.Current.MainWindow.DataContext as MainViewModel;
                mvm.CurrentPage = new PackageDetailsPage(manifest);
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

    private IEnumerable<FrameworkElement> _allPackages;

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
        set
        {
            SetProperty(ref _packages, value);
            Count = value.Count();
        }
    }

    private int _count = 0;
    public int Count
    {
        get => _count;
        set => SetProperty(ref _count, value);
    }

    private bool _filterPopupOpen = false;
    public bool FilterPopupOpen
    {
        get => _filterPopupOpen;
        set
        {
            SetProperty(ref _filterPopupOpen, value);
            FilterVisibility = value ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    private Visibility _filterVisibility = Visibility.Collapsed;
    public Visibility FilterVisibility
    {
        get => _filterVisibility;
        set => SetProperty(ref _filterVisibility, value);
    }

    private ObservableCollection<Badge> _badges = [];
    public ObservableCollection<Badge> Badges
    {
        get => _badges;
        set => SetProperty(ref _badges, value);
    }

    private int _sorting = 0;
    public int Sorting
    {
        get => _sorting;
        set
        {
            SetProperty(ref _sorting, value);

            if (value == 2)
            {
                _allPackages = _allPackages.OrderBy(p => Version.Parse(((TemplateButton)p).DescriptionText.Split("\r\n")[1].Split(":")[1]));
            }
            else
            {
                _allPackages = _allPackages.Select(f => (TemplateButton)f).OrderBy(t => value switch
                {
                    0 => t.MainText,
                    _ => t.DescriptionText.Split("\r\n")[0],
                });
            }

            if (SortDirection)
                _allPackages = _allPackages.Reverse();

            Packages = _allPackages;
            SearchQuery += ""; // Trigger setter
        }
    }

    // false: ascending
    // true: descending
    private bool _sortDirection = false;
    public bool SortDirection
    {
        get => _sortDirection;
        set
        {
            SetProperty(ref _sortDirection, value);
            _allPackages = _allPackages.Reverse();
            Packages = _allPackages;
            SearchQuery += ""; // Trigger setter
        }
    }

    private int _grouping = 0;
    public int Grouping
    {
        get => _grouping;
        set
        {
            SetProperty(ref _grouping, value);
        }
    }

    public ICommand FocusCommand => new RelayCommand<FrameworkElement>(f =>
    {
        f.Focus();
    });
}