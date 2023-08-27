using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Losch.Installer.LinFile;

/// <summary>
/// A data object that adds metadata to an installer.
/// </summary>
[Serializable]
[XmlRoot]
public class DataObject : INotifyPropertyChanged
{
    private string _name;
    /// <summary>
    /// The name of the data object.
    /// </summary>
    [XmlAttribute]
    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            PropertyChanged?.Invoke(this, new(nameof(Name)));
        }
    }

    private string _value;
    /// <summary>
    /// The data held by the data object.
    /// </summary>
    [XmlAttribute]
    public string Value
    {
        get => _value;
        set
        {
            _value = value;
            PropertyChanged?.Invoke(this, new(nameof(Value)));
        }
    }

    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    public event PropertyChangedEventHandler PropertyChanged;
}