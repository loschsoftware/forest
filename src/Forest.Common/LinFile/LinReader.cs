global using static Losch.Installer.Context;
using Losch.Installer.Behaviors;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml.Serialization;

namespace Losch.Installer.LinFile;

/// <summary>
/// Provides utilities for reading LIN files.
/// </summary>
public class LinReader
{
    /// <summary>
    /// Initializes a new instance of the <see cref="LinReader"/> type.
    /// </summary>
    /// <param name="linFile">The path to the LIN file to read.</param>
    public LinReader(string linFile)
    {
        LinFile = linFile;
        Extract();

        Current ??= new();
        GetManifest();
        GetRawFiles();
        GetCustomObjects();
        GetStrings();
        GetCustomPages();
        GetBehaviors();
    }

    /// <summary>
    /// The current LIN file.
    /// </summary>
    public string LinFile { get; set; }

    internal string WorkingDirectory { get; set; }

    internal void Extract()
    {
        if (!File.Exists(LinFile))
            return;

        string dir = Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp", "Losch", "Installer", Path.GetFileNameWithoutExtension(LinFile) + "." + Guid.NewGuid())).FullName;

        foreach (string file in Directory.GetFileSystemEntries(dir))
        {
            try
            {
                File.Delete(file);
            }
            catch (Exception) { }
        }

        try
        {
            ZipFile.ExtractToDirectory(LinFile, dir);
        }
        catch (Exception ex)
        {
            throw new LinException("Invalid LIN file.", ex);
        }

        WorkingDirectory = dir;
    }

    /// <summary>
    /// Extracts the manifest from the installer file.
    /// </summary>
    /// <returns>Returns an <see cref="InstallerManifest"/> object corresponding to the manifset of the LIN file.</returns>
    /// <exception cref="LinException"></exception>
    public InstallerManifest GetManifest()
    {
        string path = Path.Combine(WorkingDirectory, "manifest.xml");

        if (!File.Exists(path))
            throw new LinException("Invalid LIN file.");

        using FileStream fs = File.OpenRead(path);
        XmlSerializer xmls = new(typeof(InstallerManifest));

        try
        {
            InstallerManifest manifest = (InstallerManifest)xmls.Deserialize(fs);

            Current.Manifest = manifest;
            return manifest;
        }
        catch (Exception ex)
        {
            throw new LinException("Invalid LIN file.", ex);
        }
    }

    /// <summary>
    /// Retrieves the raw file data from the installer.
    /// </summary>
    /// <returns>A list of tuples containing the file name and bytes of the file.</returns>
    public List<(string FileName, byte[] Data)> GetRawFiles()
    {
        List<(string, byte[])> files = new();
        string filesDir = Path.Combine(WorkingDirectory, "Files");

        if (Directory.Exists(filesDir))
        {
            foreach (string file in Directory.GetFiles(filesDir))
                files.Add((file, File.ReadAllBytes(file)));
        }

        Current.Files = files;
        return files;
    }

    /// <summary>
    /// Retrieves all custom pages from the installer.
    /// </summary>
    /// <returns>A list of WPF pages.</returns>
    /// <exception cref="LinException"></exception>
    public List<Page> GetCustomPages()
    {
        List<Page> pages = new();
        string dir = Path.Combine(WorkingDirectory, "Pages");

        if (Directory.Exists(dir))
        {
            foreach (string file in Directory.GetFiles(dir))
            {
                using StreamReader sr = new(file);

                try
                {
                    pages.Add((Page)XamlReader.Parse(sr.ReadToEnd()));
                }
                catch (Exception ex)
                {
                    throw new LinException("Invalid LIN file.", ex);
                }
            }
        }

        Current.CustomPages = pages;
        return pages;
    }

    /// <summary>
    /// Gets all strings for installer pages.
    /// </summary>
    /// <returns>A list of tuples containing the language info and a <see cref="ResourceDictionary"/> containing the strings themselves.</returns>
    /// <exception cref="LinException"></exception>
    public List<(CultureInfo Culture, ResourceDictionary Dictionary)> GetStrings()
    {
        List<(CultureInfo, ResourceDictionary)> strings = new();
        string dir = Path.Combine(WorkingDirectory, "Strings");

        if (Directory.Exists(dir))
        {
            foreach (string file in Directory.GetFiles(dir))
            {
                try
                {
                    using StreamReader sr = new(file);
                    strings.Add((CultureInfo.GetCultureInfo(Path.GetFileNameWithoutExtension(file)), (ResourceDictionary)XamlReader.Parse(sr.ReadToEnd())));
                }
                catch (Exception ex)
                {
                    throw new LinException("Invalid LIN file.", ex);
                }
            }
        }

        Current.CustomStrings = strings;
        return strings;
    }

    /// <summary>
    /// Gets all data objects associated with the installer.
    /// </summary>
    /// <returns>A list of <see cref="DataObject"/> elements.</returns>
    public List<DataObject> GetCustomObjects()
    {
        List<DataObject> objects = new();
        string dir = Path.Combine(WorkingDirectory, "Data");
        string file = Path.Combine(dir, "objects.xml");

        if (Directory.Exists(dir) && File.Exists(file))
        {
            try
            {
                using StreamReader sr = new(file);
                XmlSerializer xmls = new(typeof(DataObjectManifest));

                DataObjectManifest manifest = (DataObjectManifest)xmls.Deserialize(sr);

                foreach (DataObject obj in manifest.Objects)
                    objects.Add(obj);
            }
            catch (Exception ex)
            {
                throw new LinException("Invalid LIN file.", ex);
            }
        }

        Current.Objects = objects;
        return objects;
    }
    
    /// <summary>
    /// Gets all behaviors associated with the installer.
    /// </summary>
    /// <returns>A list of behaviors.</returns>
    public List<(string Name, Behavior Behavior)> GetBehaviors()
    {
        List<(string, Behavior)> behaviors = new();
        string dir = Path.Combine(WorkingDirectory, "Behaviors");

        if (Directory.Exists(dir))
        {
            foreach (string file in Directory.GetFiles(dir, "*.dll").Concat(Directory.GetFiles(dir, "*.exe")))
            {
                Assembly a = Assembly.LoadFile(file);

                foreach (Type t in a.GetTypes())
                {
                    foreach (MethodInfo m in t.GetMethods())
                    {
                        if (m.IsStatic 
                            && m.ReturnType == typeof(int)
                            && m.GetParameters().Select(p => p.ParameterType).ToArray().SequenceEqual(new Type[] { typeof(Dictionary<string, string>) }))
                        {
                            behaviors.Add((m.Name, new(m.CreateDelegate<Behavior>())));
                        }
                    }
                }
            }
        }

        Current.Behaviors = behaviors;
        return behaviors;
    }
}