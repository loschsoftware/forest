using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Forest.UI.ViewModels;
using Forest.UI.Views;
using Losch.Installer;
using Losch.Installer.Behaviors;
using Losch.Installer.LinFile;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Forest.ViewModels;

public class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        ObservableCollection<Page> pages = new()
        {
            new StartPage(this)
        };

        Pages = pages;
    }

    private int _pageIndex = 0;

    private int _index = 0;

    private int _pageDepth = 0;
    private Dictionary<int, List<InstallerPage>> _installerPages = new();
    public Dictionary<int, List<InstallerPage>> InstallerPages
    {
        get => _installerPages;
        set
        {
            SetProperty(ref _installerPages, value);

            ObservableCollection<Page> pages = new();
            foreach (InstallerPage page in value[0])
                pages.Add(App.GetPage(page.Id));

            Pages = pages;
        }
    }

    private ObservableCollection<Page> _pages = new();
    public ObservableCollection<Page> Pages
    {
        get => _pages;
        set
        {
            SetProperty(ref _pages, value);
            CurrentPage = Pages.First();
        }
    }

    private Page _currentPage;
    public Page CurrentPage
    {
        get => _currentPage;
        set
        {
            SetProperty(ref _currentPage, value);
            Title = value.Title;

            if (value is LibraryPage)
                StatusText = $"{string.Format((string)Application.Current.TryFindResource("StringNPackagesInstalled"), ((value as LibraryPage).DataContext as LibraryViewModel).Count.ToString())} • {string.Format((string)Application.Current.TryFindResource("StringTotalSize"), "0 B")}";
            else
                StatusText = "";
        }
    }

    private string _title = (string)Application.Current.TryFindResource("StringAppTitle");
    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    private Visibility _nextButtonVisibility = Visibility.Collapsed;
    public Visibility NextButtonVisibility
    {
        get => _nextButtonVisibility;
        set => SetProperty(ref _nextButtonVisibility, value);
    }

    private Visibility _backButtonVisibility = Visibility.Collapsed;
    public Visibility BackButtonVisibility
    {
        get => _backButtonVisibility;
        set => SetProperty(ref _backButtonVisibility, value);
    }

    private bool _isBackButtonEnabled = false;
    public bool IsBackButtonEnabled
    {
        get => _isBackButtonEnabled;
        set => SetProperty(ref _isBackButtonEnabled, value);
    }

    private string _closeButtonText = (string)Application.Current.TryFindResource("StringButtonClose");
    public string CloseButtonText
    {
        get => _closeButtonText;
        set => SetProperty(ref _closeButtonText, value);
    }

    private Visibility _startPageButtonVisibility = Visibility.Visible;
    public Visibility StartPageButtonVisibility
    {
        get => _startPageButtonVisibility;
        set => SetProperty(ref _startPageButtonVisibility, value);
    }

    public string CopyrightString { get; set; } = $"© {DateTime.Now.Year} Losch";

    private double _width = 500;
    public double Width
    {
        get => _width;
        set => SetProperty(ref _width, value);
    }

    private double _height = 500;
    public double Height
    {
        get => _height;
        set => SetProperty(ref _height, value);
    }

    private string _statusText = "";
    public string StatusText
    {
        get => _statusText;
        set => SetProperty(ref _statusText, value);
    }

    public ICommand CloseCommand => new RelayCommand(Application.Current.Shutdown);

    public ICommand NavigateToLastPage => new RelayCommand(() =>
    {
        CurrentPage = _prevPage ?? new StartPage(this);
    });

    Page _prevPage;

    public ICommand ShowAboutCommand => new RelayCommand(() =>
    {
        _prevPage = CurrentPage;
        CurrentPage = new AboutPage();
    });

    public ICommand ShowMainPageCommand => new RelayCommand(() =>
    {
        _prevPage = CurrentPage;
        CurrentPage = new StartPage(this);
        Width = 500;
        Height = 500;
    });

    public ICommand ShowLibraryCommand => new RelayCommand(() =>
    {
        CurrentPage = new LibraryPage();
        Width = 600;
        Height = 600;
    });

    public ICommand ShowInstallApplicationPageCommand => new RelayCommand(() =>
    {
        CurrentPage = new InstallApplicationPage();
        Width = 500;
        Height = 500;
    });

    public ICommand BackCommand => new RelayCommand(() =>
    {
        CloseButtonText = (string)Application.Current.TryFindResource("StringButtonClose");
        NextButtonVisibility = Visibility.Visible;

        if (_pageIndex - 1 < 0)
        {
            IsBackButtonEnabled = false;
            return;
        }

        CurrentPage = Pages[--_pageIndex];

        if (_pageIndex - 1 < 0)
            IsBackButtonEnabled = false;
    });

    List<(SerializedBehavior, Behavior)> _postBehaviors = new();

    public ICommand NextCommand => new RelayCommand(() =>
    {
        if (Pages.Count <= _pageIndex + 1)
        {
            NextButtonVisibility = Visibility.Collapsed;
            CloseButtonText = (string)Application.Current.TryFindResource("StringButtonFinish");
            return;
        }

        foreach ((SerializedBehavior serializedBehavior, Behavior behavior) in _postBehaviors)
        {
            Dictionary<string, string> objects = new();

            foreach (var obj in Context.Current.Objects)
                objects.Add(obj.Name, obj.Value);

            int ret = behavior.Invoke(objects);

            if (ret != 0 && serializedBehavior.IsCritical)
            {
                AdonisUI.Controls.MessageBox.Show(
                    string.Format((string)Application.Current.TryFindResource("StringBehaviorError"), ret),
                    (string)Application.Current.TryFindResource("StringAppTitle"),
                    AdonisUI.Controls.MessageBoxButton.OK,
                    AdonisUI.Controls.MessageBoxImage.Error);

                Application.Current.Shutdown();
            }
        }

        _postBehaviors.Clear();

        if (InstallerPages[_pageDepth].Count > _index /*&& (_index == 0 || Pages.Count <= _pageIndex + 1)*/)
        {
            InstallerPage current = InstallerPages[_pageDepth][_index++];

            foreach (SerializedBehavior behavior in current.Behaviors ?? Array.Empty<SerializedBehavior>())
            {
                if (Context.Current.Behaviors.Any(b => b.Name == behavior.Name))
                {
                    Dictionary<string, string> objects = new();

                    foreach (var obj in Context.Current.Objects)
                        objects.Add(obj.Name, obj.Value);

                    Context.Current.Behaviors.Where(b => b.Name == behavior.Name).ToList()
                    .ForEach(b =>
                    {
                        if (behavior is PreBehavior)
                            b.Behavior.Invoke(objects);

                        else if (behavior is PostBehavior)
                            _postBehaviors.Add((behavior, b.Behavior));

                    });
                }
            }

            if (current.Cases != null && current.Cases.Any())
            {
                foreach (Case _case in current.Cases)
                {
                    if (ResourceProvider.GetObject(_case.Object ??= "") == (_case.Data ??= "") || _case is Else)
                    {
                        InstallerPages.Add(++_pageDepth, _case.Pages.ToList());

                        for (int i = 0; i < _case.Pages.Length; i++)
                            Pages.Insert(_index + i, App.GetPage(_case.Pages[i].Id));
                    }
                }
            }
        }

        CurrentPage = Pages[++_pageIndex];
        IsBackButtonEnabled = true;

        if (Pages.Count <= _pageIndex + 1)
        {
            NextButtonVisibility = Visibility.Collapsed;
            CloseButtonText = (string)Application.Current.TryFindResource("StringButtonFinish");
        }
    });
}