using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Forest.UI.Views;
using System;
using System.Collections.ObjectModel;
using System.IO;
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

        InstallerPages = pages;
    }

    private int _pageIndex = 0;

    private ObservableCollection<Page> _installerPages = new();
    public ObservableCollection<Page> InstallerPages
    {
        get => _installerPages;
        set
        {
            SetProperty(ref _installerPages, value);
            CurrentPage = InstallerPages.First();
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

    public string CopyrightString { get; set; } = $"© {DateTime.Now.Year} Losch";

    public ICommand CloseCommand => new RelayCommand(Application.Current.Shutdown);

    public ICommand BackToMainPageCommand => new RelayCommand(() =>
    {
        CurrentPage = _prevPage;
    });

    Page _prevPage;

    public ICommand ShowAboutCommand => new RelayCommand(() =>
    {
        _prevPage = CurrentPage;
        CurrentPage = new AboutPage();
    });

    public ICommand ShowLibraryCommand => new RelayCommand(() =>
    {
        CurrentPage = new LibraryPage();
    });

    public ICommand ShowInstallApplicationPageCommand => new RelayCommand(() =>
    {
        CurrentPage = new InstallApplicationPage();
    });

    public ICommand BackCommand => new RelayCommand(() =>
    {
        CurrentPage = InstallerPages[--_pageIndex];

        if (_pageIndex < 0)
            IsBackButtonEnabled = false;
    });

    public ICommand NextCommand => new RelayCommand(() =>
    {
        if (InstallerPages.Count <= _pageIndex + 1)
        {
            NextButtonVisibility = Visibility.Collapsed;
            CloseButtonText = (string)Application.Current.TryFindResource("StringButtonFinish");
            return;
        }

        CurrentPage = InstallerPages[++_pageIndex];

        if (InstallerPages.Count <= _pageIndex + 1)
        {
            NextButtonVisibility = Visibility.Collapsed;
            CloseButtonText = (string)Application.Current.TryFindResource("StringButtonFinish");
        }
    });
}