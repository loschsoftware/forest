using AdonisUI.Controls;
using System.Windows.Navigation;

namespace Forest.Views;

public partial class MainView : AdonisWindow
{
    public MainView()
    {
        InitializeComponent();
    }

    private void Frame_Navigating(object sender, NavigatingCancelEventArgs e)
    {
        //(e.Content as Page).BeginAnimation(OpacityProperty, new DoubleAnimation(0, 1, TimeSpan.FromSeconds(1)));
    }
}