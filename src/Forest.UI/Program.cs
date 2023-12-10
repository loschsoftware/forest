using System;
using System.Globalization;
using System.Windows;

namespace Forest.UI;

internal class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        try
        {
            App app = new();
            app.InitializeComponent();

            if (CultureInfo.CurrentCulture.ThreeLetterISOLanguageName == "deu")
                Application.Current.Resources.MergedDictionaries.RemoveAt(1);

            app.Run();
        }
        catch (Exception ex)
        {
            AdonisUI.Controls.MessageBox.Show(string.Format((string)Application.Current.TryFindResource("StringException"), ex.ToString()), (string)Application.Current.TryFindResource("StringAppTitle"), AdonisUI.Controls.MessageBoxButton.OK, AdonisUI.Controls.MessageBoxImage.Error);
        }
    }
}