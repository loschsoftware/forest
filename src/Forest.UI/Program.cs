using System;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace Forest.UI;

internal class Program
{
    internal static string package;
    internal static bool disableLaunchButton;

    [STAThread]
    static void Main(string[] args)
    {
        try
        {
            if (args.Any(a => a.StartsWith("-id:")))
                package = args.First(a => a.StartsWith("-id:")).Split(':')[1];

            if (args.Any(a => a == "-norun"))
                disableLaunchButton = true;

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