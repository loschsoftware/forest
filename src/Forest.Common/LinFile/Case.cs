using System;
using System.Xml.Serialization;

namespace Losch.Installer.LinFile;

/// <summary>
/// Allows setting different installation steps based on a condition.
/// </summary>
[XmlRoot]
[Serializable]
public class Case
{
    /// <summary>
    /// The data object to compare.
    /// </summary>
    [XmlAttribute]
    public string Object { get; set; }

    /// <summary>
    /// The data to compare the object with.
    /// </summary>
    [XmlAttribute]
    public string Data { get; set; }

    /// <summary>
    /// An array of pages that will be shown if the case is fulfilled.
    /// </summary>
    [XmlArray]
    public InstallerPage[] Pages { get; set; }
}

/// <summary>
/// The pages that will be shown if no case is fulfilled.
/// </summary>
[XmlRoot]
[Serializable]
public class Else : Case
{

}