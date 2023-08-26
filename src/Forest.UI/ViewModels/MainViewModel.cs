using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Forest.UI.Views;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Forest.ViewModels;

public class MainViewModel : ObservableObject
{
    public MainViewModel()
    {
        _currentPage = new StartPage(this);
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

    public string CopyrightString { get; set; } = $"© {DateTime.Now.Year} Losch";

    public ICommand CloseCommand => new RelayCommand(Application.Current.Shutdown);

    public ICommand BackToMainPageCommand => new RelayCommand(() =>
    {
        CurrentPage = new StartPage(this);
    });

    public ICommand ShowAboutCommand => new RelayCommand(() =>
    {
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
}