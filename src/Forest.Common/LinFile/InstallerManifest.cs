using System;
using System.Xml.Serialization;

namespace Losch.Installer.LinFile;

/// <summary>
/// The main manifest of a LIN installer file.
/// </summary>
[Serializable]
[XmlRoot("Installer")]
public class InstallerManifest
{
    /// <summary>
    /// Specifies the version of the manifest.
    /// </summary>
    [XmlAttribute]
    public string FormatVersion { get; set; }

    /// <summary>
    /// The web location containing the files needed to update the package. 
    /// </summary>
    public string InstallerSource { get; set; }

    /// <summary>
    /// The installer package information.
    /// </summary>
    public Package Package { get; set; }

    /// <summary>
    /// The pages making up the installation process.
    /// </summary>
    [XmlArray("InstallerSteps")]
    [XmlArrayItem(typeof(DefaultPage))]
    [XmlArrayItem(typeof(CustomPage))]
    [XmlArrayItem(typeof(InstallationProcedure))]
    public InstallerPage[] Steps { get; set; } 
}