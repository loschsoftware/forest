using System;
using System.Xml.Serialization;

namespace Losch.Installer.LinFile;

/// <summary>
/// Represents an installer package.
/// </summary>
[Serializable]
[XmlRoot]
public class Package
{
    /// <summary>
    /// The ID of the package.
    /// </summary>
    public string UniqueId { get; set; }
}