using System;
using System.Xml.Serialization;

namespace Losch.Installer.LinFile;

/// <summary>
/// A manifest containing data objects to be used by an installer.
/// </summary>
[XmlRoot("InstallerObjects")]
[Serializable]
public class DataObjectManifest
{
    /// <summary>
    /// The objects used by the installer.
    /// </summary>
    [XmlArray]
    public DataObject[] Objects { get; set; }
}