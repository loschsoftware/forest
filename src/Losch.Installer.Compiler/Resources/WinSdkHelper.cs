using System;
using System.IO;

namespace Losch.Installer.Compiler.Resources;

internal static class WinSdkHelper
{
    public static string GetToolPath(string tool)
    {
        string winSdkBaseDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Windows Kits", "10", "bin");

        if (!Directory.Exists(winSdkBaseDir))
            return "";

        string[] files = Directory.GetFiles(winSdkBaseDir, tool, SearchOption.AllDirectories);

        // TODO: This is an exceptionally bad implementation

        if (files.Length > 1)
            return files[1]; // Skips ARM version

        return "";
    }
}