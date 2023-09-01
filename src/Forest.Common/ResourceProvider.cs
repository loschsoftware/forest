using Losch.Installer.LinFile;
using System;
using System.CodeDom;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace Losch.Installer;

internal static class ResourceProvider
{
    public static string GetString(string stringId, CultureInfo culture)
    {
        try
        {
            return Current.CustomStrings.First(s => s.Culture.ThreeLetterISOLanguageName == culture.ThreeLetterISOLanguageName && s.Dictionary.Contains(stringId)).Dictionary[stringId].ToString();
        }
        catch (Exception)
        {
            return "";
        }
    }

    public static Page GetCustomPage(string id)
    {
        try
        {
            return Current.CustomPages.First(p => p.Name == id);
        }
        catch (Exception)
        {
            return new();
        }
    }

    public static DataObject GetDataObject(string id)
    {
        try
        {
            if (Current.DefaultObjects.Any(d => d.Name == id))
                return Current.DefaultObjects.First(d => d.Name == id);

            return Current.Objects.First(o => o.Name == id);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static string GetObject(string id)
    {
        try
        {
            if (Current.DefaultObjects.Any(d => d.Name == id))
                return Current.DefaultObjects.First(d => d.Name == id).Value;

            return Current.Objects.First(o => o.Name == id).Value;
        }
        catch (Exception)
        {
            return "";
        }
    }

    public static void SetObject(string id, string value)
    {
        try
        {
            if (Current.DefaultObjects.Any(d => d.Name == id))
                Current.DefaultObjects.First(o => o.Name == id).Value = value;
            else
                Current.Objects.First(o => o.Name == id).Value = value;
        }
        catch (Exception) { }
    }
}