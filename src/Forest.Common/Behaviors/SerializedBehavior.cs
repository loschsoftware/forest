using System;
using System.Xml.Serialization;

namespace Losch.Installer.Behaviors;

/// <summary>
/// Represents a behavior run at the opening or closing of an installer page.
/// </summary>
[Serializable]
[XmlRoot("Behavior")]
public abstract class SerializedBehavior
{
    /// <summary>
    /// The name of the behavior.
    /// </summary>
    [XmlText]
    public string Name { get; set; }

    /// <summary>
    /// Specifies wheter the behavior should be run again when the installer navigates back to the page of the behavior.
    /// </summary>
    [XmlAttribute("Repeat")]
    public bool RunRepeated { get; set; }

    /// <summary>
    /// Specifies wheter the installer should stop if the behavior fails to execute correctly.
    /// </summary>
    [XmlAttribute("Critical")]
    public bool IsCritical { get; set; }
}

/// <summary>
/// Represents a behavior that is run when an installer page is loaded.
/// </summary>
[Serializable]
[XmlRoot]
public class PreBehavior : SerializedBehavior { }

/// <summary>
/// Represents a behavior that is run after an installer page has been unloaded.
/// </summary>
[Serializable]
[XmlRoot]
public class PostBehavior  : SerializedBehavior { }

/// <summary>
/// An indexed behavior that is run during the installation process itself.
/// </summary>
[Serializable]
[XmlRoot]
public class InstallationBehavior : SerializedBehavior
{
    /// <summary>
    /// The index of the behavior determines the order of the installation behaviors run during the installation process.
    /// </summary>
    [XmlAttribute]
    public int Index { get; set; }
}