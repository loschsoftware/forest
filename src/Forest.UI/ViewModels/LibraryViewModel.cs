using CommunityToolkit.Mvvm.ComponentModel;
using Losch.LSEdit.Core.UI;
using System.Collections.ObjectModel;

namespace Forest.UI.ViewModels;

public class LibraryViewModel : ObservableObject
{
    private string _searchQuery;
    public string SearchQuery
    {
        get => _searchQuery;
        set => SetProperty(ref _searchQuery, value);
    }

    private ObservableCollection<TemplateButton> _packages = [];
    public ObservableCollection<TemplateButton> Packages
    {
        get => _packages;
        set => SetProperty(ref _packages, value);
    }
}