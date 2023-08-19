using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Forest.CLI;

internal class Program
{
    static string[] _args;

    [STAThread]
    static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.Unicode;
        Console.InputEncoding = Encoding.Unicode;

        if (args == null || args.Length == 0)
            args = new string[] { "help", "" };

        if (args.Length == 1)
            args = args.Append("").ToArray();

        _args = args[1..];

        switch (args[0])
        {
            case "available":
                ListAvailablePackages();
                break;

            case "uninstall":
                UninstallPackage();
                break;

            case "update":
                UpdatePackage();
                break;

            case "list":
                ListInstalledPackages();
                break;

            case "install":
                InstallPackage();
                break;

            default:
            case "ui":
                StartUI();
                break;

            case "help":
                ShowHelp();
                break;
        }
    }

    private static void ShowHelp()
    {
        ConsoleColor prev = Console.ForegroundColor;

        Console.ForegroundColor = ConsoleColor.Yellow;

        Console.WriteLine("Losch Installer Command Line Interface (forest.exe)");
        Console.WriteLine($"Version {Assembly.GetExecutingAssembly().GetName().Version.ToString(2)}, © {DateTime.Now.Year} Losch");
        Console.WriteLine();

        Console.ForegroundColor = prev;

        Console.WriteLine("The following commands are available:");

        Console.WriteLine($"{"    available",-50}Lists all available packages.");
        Console.WriteLine($"{"    uninstall <PackageId>",-50}Uninstalls the specified package.");
        Console.WriteLine($"{"    update <PackageId>",-50}Updates the specified package to the newest version.");
        Console.WriteLine($"{"    list",-50}Lists all installed packages.");
        Console.WriteLine($"{"    install <PackageId>",-50}Installs the specified package.");
        Console.WriteLine($"{"    ui",-50}Starts the graphical user interface of the package manager.");
        Console.WriteLine($"{"    help",-50}Shows this page.");
    }

    private static void ListAvailablePackages()
    {
        throw new NotImplementedException();
    }

    private static void UninstallPackage()
    {
        throw new NotImplementedException();
    }

    private static void UpdatePackage()
    {
        throw new NotImplementedException();
    }

    private static void ListInstalledPackages()
    {
        throw new NotImplementedException();
    }

    private static void InstallPackage()
    {
        throw new NotImplementedException();
    }

    private static void StartUI()
    {
        Process.Start("forestui.exe");
    }
}