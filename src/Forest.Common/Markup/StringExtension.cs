using System;
using System.Globalization;
using System.Windows.Markup;

namespace Losch.Installer.Markup;

/// <summary>
/// A markup extension to access installer strings inside of a custom page.
/// </summary>
public class StringExtension : MarkupExtension
{
    /// <summary>
    /// The ID of the string to retrieve.
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="StringExtension"/> type.
    /// </summary>
    /// <param name="id">The ID of the string to retrieve.</param>
    public StringExtension(string id) => Id = id;
    /// <summary>
    /// Initializes a new instance of the <see cref="StringExtension"/> type.
    /// </summary>
    public StringExtension() { }

    /// <summary>
    /// Retrieves the string value corresponding to the string with the specified ID.
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns>The string value corresponding to the string with the specified ID.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        string str = ResourceProvider.GetString(Id, CultureInfo.CurrentCulture);

        if (string.IsNullOrEmpty(str))
            ResourceProvider.GetString(Id, CultureInfo.GetCultureInfo("en-US"));

        return str; 
    }
}