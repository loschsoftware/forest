using System;
using System.Globalization;
using System.Windows;

namespace Forest.CLI;

internal class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        App app = new();
        app.InitializeComponent();

        if (CultureInfo.CurrentCulture.Name.StartsWith("de-"))
            Application.Current.Resources.MergedDictionaries.RemoveAt(1);

        app.Run();
    }
}