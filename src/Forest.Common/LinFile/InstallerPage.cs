using System;
using System.Xml.Serialization;

namespace Losch.Installer.LinFile;

/// <summary>
/// A page of an installation process.
/// </summary>
[Serializable]
public abstract class InstallerPage
{
    /// <summary>
    /// The unique identifier of the page.
    /// </summary>
    [XmlAttribute]
    public string Id { get; set; }

    /// <summary>
    /// Cases for conditional pages.
    /// </summary>
    [XmlArray]
    public Case[] Cases { get; set; }
}

/// <summary>
/// A default page included in the Losch Installer runtime.
/// </summary>
[Serializable]
[XmlRoot]
public class DefaultPage : InstallerPage { }

/// <summary>
/// A user-defined custom page.
/// </summary>
[Serializable]
[XmlRoot]
public class CustomPage : InstallerPage { }

/// <summary>
/// The most important step of the installation procedure; the page that shows while the actual installation is in progress.
/// </summary>
[Serializable]
[XmlRoot]
public class InstallationProcedure : InstallerPage { }