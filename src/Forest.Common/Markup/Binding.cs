using System.Windows;

namespace Losch.Installer.Markup;

/// <summary>
/// Attached properties used to access data objects inside of an installer page.
/// </summary>
public class Binding : DependencyObject
{
    /// <summary>
    /// The data object to bind to.
    /// </summary>
    public static readonly DependencyProperty ObjectProperty = DependencyProperty.Register(
        "Object",
        typeof(string),
        typeof(Binding));

    /// <summary>
    /// Gets the value of the <see cref="ObjectProperty"/> dependency property.
    /// </summary>
    /// <param name="obj">The <see cref="DependencyObject"/> to get the value from.</param>
    /// <returns>The value of the property.</returns>
    public static string GetObject(DependencyObject obj) => (string)obj.GetValue(ObjectProperty);

    /// <summary>
    /// Sets the value of the <see cref="ObjectProperty"/> dependency property.
    /// </summary>
    /// <param name="obj">The <see cref="DependencyObject"/> to get the value from.</param>
    /// <param name="value">The new value of the property.</param>
    public static void SetObject(DependencyObject obj, string value) => obj.SetValue(ObjectProperty, value);
}