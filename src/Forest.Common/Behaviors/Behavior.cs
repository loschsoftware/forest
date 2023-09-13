using System.Collections.Generic;

namespace Losch.Installer.Behaviors;

/// <summary>
/// A delegate representing an installation behavior.
/// </summary>
/// <param name="dataObjects">The data objects available during the call of the behavior.</param>
/// <returns>An error code indicating wheter the operation was successful.</returns>
public delegate int Behavior(Dictionary<string, string> dataObjects);