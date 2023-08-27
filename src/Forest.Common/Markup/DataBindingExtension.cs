using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Losch.Installer.Markup;

/// <summary>
/// A markup extension to bind to data objects.
/// </summary>
[MarkupExtensionReturnType(typeof(object))]
public class DataBindingExtension : MarkupExtension
{
    /// <summary>
    /// The name of the data object to bind to.
    /// </summary>
    public string ObjectName { get; set; }

    /// <summary>
    /// Creates a new instance of the <see cref="DataBindingExtension"/> type with the specified data object name.
    /// </summary>
    /// <param name="objectName">The name of the data object.</param>
    public DataBindingExtension(string objectName) => ObjectName = objectName;
    /// <summary>
    /// Creates a new instance of the <see cref="DataBindingExtension"/> type.
    /// </summary>
    public DataBindingExtension() { }

    /// <summary>
    /// Evaluates the binding.
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <returns>The value of the data object corresponding to the name specified in <see cref="ObjectName"/>.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        if (string.IsNullOrEmpty(ObjectName))
            return null;

        IProvideValueTarget value = (IProvideValueTarget)serviceProvider.GetService(typeof(IProvideValueTarget));
        DependencyProperty property = (DependencyProperty)value.TargetProperty;
        FrameworkElement obj = (FrameworkElement)value.TargetObject;

        Binding binding = new("Value")
        {
            Source = ResourceProvider.GetDataObject(ObjectName),
            Mode = BindingMode.TwoWay,
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
        };

        obj.SetBinding(property, binding);

        return Convert.ChangeType(ResourceProvider.GetObject(ObjectName), property.PropertyType);
    }
}