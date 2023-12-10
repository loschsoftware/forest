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
}