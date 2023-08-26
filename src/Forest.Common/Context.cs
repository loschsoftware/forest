using Losch.Installer.LinFile;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Globalization;

namespace Losch.Installer;

internal class Context
{
    public Context() => Current = this;

    public static Context Current { get; set; } = new();

    public InstallerManifest Manifest { get; set; }

    public List<Page> CustomPages { get; set; }
    
    public List<(CultureInfo Culture, ResourceDictionary Dictionary)> CustomStrings { get; set; }

    public List<(string FileName, byte[] Data)> Files { get; set; }
}