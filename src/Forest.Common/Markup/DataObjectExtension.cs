using System;
using System.Windows.Markup;

namespace Losch.Installer.Markup;

/// <summary>
/// Markup extension for accessing object values inside of an installer page.
/// </summary>
public class DataObjectExtension : MarkupExtension
{
    /// <summary>
    /// The name of the data object.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="DataObjectExtension"/> type with an object name.
    /// </summary>
    /// <param name="name">The name of the object.</param>
    public DataObjectExtension(string name) => Name = name;
    /// <summary>
    /// Initializes a new instance of the <see cref="DataObjectExtension"/> type.
    /// </summary>
    public DataObjectExtension() { }

    /// <summary>
    /// Retrieves the value corresponding to the specified data object.
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns>The value corresponding to the specified data object.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return ResourceProvider.GetObject(Name);
    }
}