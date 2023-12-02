using System;
using System.Runtime.Serialization;

namespace Losch.Installer.LinFile;

/// <summary>
/// Exception thrown when a LIN file is invalid.
/// </summary>
[Serializable]
public class LinException : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="LinException"/> type.
	/// </summary>
	public LinException() { }

    /// <summary>
    /// Initializes a new instance of the <see cref="LinException"/> type.
    /// </summary>
    /// <param name="message">The exception message.</param>
    public LinException(string message) : base(message) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="LinException"/> type.
    /// </summary>
    /// <param name="message">The exception message.</param>
    /// <param name="inner">The inner exception.</param>
    public LinException(string message, Exception inner) : base(message, inner) { }
}