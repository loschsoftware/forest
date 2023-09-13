using Losch.Installer.LinFile;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Windows;
using System.Globalization;
using DataObject = Losch.Installer.LinFile.DataObject;
using Losch.Installer.Behaviors;

namespace Losch.Installer;

internal class Context
{
    public Context() => Current = this;

    public static Context Current { get; set; } = new();

    public InstallerManifest Manifest { get; set; }

    public List<Page> CustomPages { get; set; }
    
    public List<(CultureInfo Culture, ResourceDictionary Dictionary)> CustomStrings { get; set; }

    public List<(string FileName, byte[] Data)> Files { get; set; }

    public List<DataObject> Objects { get; set; }

    public List<DataObject> DefaultObjects => new()
    {
        new()
        {
            Name = "InstallationPath"
        },

        new()
        {
            Name = "StartAfterInstall",
            Value = "true"
        }
    };

    public List<(string Name, Behavior Behavior)> Behaviors { get; set; }
}