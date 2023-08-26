using System;
using System.Globalization;
using System.Linq;

namespace Losch.Installer;

internal static class ResourceProvider
{
    public static string GetString(string stringId, CultureInfo culture)
    {
		try
		{
			return Current.CustomStrings.Where(s => s.Culture.Name == culture.Name && s.Dictionary.Contains(stringId)).First().Dictionary[stringId].ToString();
		}
		catch (Exception)
		{
			return "";
		}
    }
}