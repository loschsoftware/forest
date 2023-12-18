using System;
using System.Xml.Serialization;

namespace Losch.Installer.PackageManifest;

/// <summary>
/// Specifies basic information about a package.
/// </summary>
[XmlRoot]
[Serializable]
public class PackageInfo
{
    /// <summary>
    /// Specifies the unique id of the package.
    /// </summary>
    [XmlElement("Id")]
    public string UniqueId { get; set; }

    /// <summary>
    /// Specifies the name of the package.
    /// </summary>
    [XmlElement("Name")]
    public string PackageName { get; set; }

    /// <summary>
    /// Specifies the package author.
    /// </summary>
    [XmlElement]
    public string Author { get; set; }

    /// <summary>
    /// Specifies the package publisher.
    /// </summary>
    [XmlElement]
    public string Publisher { get; set; }

    /// <summary>
    /// Specifies the path to the icon of the package.
    /// </summary>
    [XmlElement("Icon")]
    public string IconFile { get; set; }
}