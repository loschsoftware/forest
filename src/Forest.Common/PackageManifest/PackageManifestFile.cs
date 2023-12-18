using System;
using System.Xml.Serialization;

namespace Losch.Installer.PackageManifest;

/// <summary>
/// Represents an entry in the list of installed packages.
/// </summary>
[XmlRoot("LinPackage")]
[Serializable]
public class PackageManifestFile
{
    /// <summary>
    /// Represents the version of the manifest file format.
    /// </summary>
    [XmlAttribute]
    public string FormatVersion { get; set; }

    /// <summary>
    /// Specifies basic information about the package.
    /// </summary>
    [XmlElement("PackageIdentity")]
    public PackageInfo PackageInfo { get; set; }

    /// <summary>
    /// Specifies wheter the application can be started from the Installer UI.
    /// </summary>
    [XmlElement]
    public bool CanStart { get; set; }

    /// <summary>
    /// Specifies the command executed when the application is started from the Installer UI.
    /// </summary>
    [XmlElement]
    public string StartupAction { get; set; }

    /// <summary>
    /// Specifies the package installation date.
    /// </summary>
    [XmlElement]
    public string InstallationDate { get; set; }

    /// <summary>
    /// Specifies the base directory of application-related files.
    /// </summary>
    [XmlElement]
    public string BaseDirectory { get; set; }

    /// <summary>
    /// Specifies the package version.
    /// </summary>
    [XmlElement]
    public string Version { get; set; }
}