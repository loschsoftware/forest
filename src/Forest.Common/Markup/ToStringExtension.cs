using System;
using System.Windows.Markup;

namespace Losch.Installer.Markup;

/// <summary>
/// A markup extension to convert any object to a string.
/// </summary>
public class ToStringExtension : MarkupExtension
{
    /// <summary>
    /// The value to convert.
    /// </summary>
    public object Value { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ToStringExtension"/> type with the value to convert.
    /// </summary>
    /// <param name="value">The name of the object.</param>
    public ToStringExtension(object value) => Value = value;
    /// <summary>
    /// Initializes a new instance of the <see cref="ToStringExtension"/> type.
    /// </summary>
    public ToStringExtension() { }

    /// <summary>
    /// Converts the specified value to a string.
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns>The converted string.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider) => Value.ToString();
}