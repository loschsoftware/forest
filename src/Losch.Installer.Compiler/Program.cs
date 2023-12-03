using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows;

namespace Losch.Installer.Compiler;

internal class Program
{
    private static int Main(string[] args) => args switch
    {
        [string path, ..] => Compile(path, args[1..]),
        _ => PrintUsage()
    };

    private static void PrintLogo()
    {
        Console.WriteLine("Losch Installer Bootstrapping Compiler");
        Console.WriteLine("Version 1.0, (C) 2023 Losch");
        Console.WriteLine();
    }

    private static int Compile(string path, string[] options)
    {
        PrintLogo();

        string outFile = Path.ChangeExtension(path, ".exe");
        if (options.Any(o => o.StartsWith("-out:")))
            outFile = string.Join("", options.First(o => o.StartsWith("-out:")).Split(':')[1..]);

        string title = "";
        string copyright = "";
        string company = "";
        string version = "";

        if (options.Any(o => o.StartsWith("-title:")))
            title = string.Join("", options.First(o => o.StartsWith("-title:")).Split(':')[1..]);

        if (options.Any(o => o.StartsWith("-copyright:")))
            copyright = string.Join("", options.First(o => o.StartsWith("-copyright:")).Split(':')[1..]);

        if (options.Any(o => o.StartsWith("-company:")))
            company = string.Join("", options.First(o => o.StartsWith("-company:")).Split(':')[1..]);

        if (options.Any(o => o.StartsWith("-version:")))
            version = string.Join("", options.First(o => o.StartsWith("-version:")).Split(':')[1..]);

        Stopwatch sw = new();
        sw.Start();

        if (!File.Exists(path))
        {
            Console.WriteLine("Error: Specified input file does not exist.");
            return -1;
        }

        Console.WriteLine($"Info: Compiling file '{path}', generating '{outFile}'.");

        byte[] blob = File.ReadAllBytes(path);
        MemoryStream linStream = new(blob);

        AssemblyName name = new(Path.GetFileNameWithoutExtension(outFile));
        AssemblyBuilder ab = AssemblyBuilder.DefineDynamicAssembly(name, AssemblyBuilderAccess.RunAndSave);

        ModuleBuilder mb = ab.DefineDynamicModule(outFile, outFile, false);
        mb.DefineManifestResource("linfile", linStream, ResourceAttributes.Public);

        TypeBuilder tb = mb.DefineType("Entry", TypeAttributes.NotPublic);
        MethodBuilder main = tb.DefineMethod("Main", MethodAttributes.Static, CallingConventions.Standard, typeof(void), [typeof(string[])]);

        ILGenerator il = main.GetILGenerator();

        il.DeclareLocal(typeof(string)); // 0
        il.DeclareLocal(typeof(string)); // 1

        il.Emit(OpCodes.Ldc_I4, (int)Environment.SpecialFolder.LocalApplicationData);
        il.Emit(OpCodes.Call, typeof(Environment).GetMethod("GetFolderPath", BindingFlags.Static | BindingFlags.Public, null, [typeof(Environment.SpecialFolder)], null));
        il.Emit(OpCodes.Ldstr, "Temp");
        il.Emit(OpCodes.Ldstr, "lin");
        il.Emit(OpCodes.Call, typeof(Path).GetMethod("Combine", BindingFlags.Public | BindingFlags.Static, null, [typeof(string), typeof(string), typeof(string)], null));
        il.Emit(OpCodes.Stloc_S, (byte)0);

        Label after = il.DefineLabel();
        il.Emit(OpCodes.Ldloc_S, (byte)0);
        il.Emit(OpCodes.Call, typeof(Directory).GetMethod("Exists", BindingFlags.Public | BindingFlags.Static, null, [typeof(string)], null));
        il.Emit(OpCodes.Brfalse_S, after);

        il.Emit(OpCodes.Ldloc_S, (byte)0);
        il.Emit(OpCodes.Newobj, typeof(DirectoryInfo).GetConstructor([typeof(string)]));
        il.Emit(OpCodes.Ldc_I4_S, (byte)1);
        il.Emit(OpCodes.Callvirt, typeof(DirectoryInfo).GetMethod("Delete", BindingFlags.Public | BindingFlags.Instance, null, [typeof(bool)], null));

        il.MarkLabel(after);
        il.Emit(OpCodes.Ldloc_S, (byte)0);
        il.Emit(OpCodes.Call, typeof(Directory).GetMethod("CreateDirectory", BindingFlags.Public | BindingFlags.Static, null, [typeof(string)], null));
        il.Emit(OpCodes.Pop);

        il.Emit(OpCodes.Ldloc_S, (byte)0);
        il.Emit(OpCodes.Ldstr, "linui.zip");
        il.Emit(OpCodes.Call, typeof(Path).GetMethod("Combine", BindingFlags.Public | BindingFlags.Static, null, [typeof(string), typeof(string)], null));
        il.Emit(OpCodes.Stloc_S, (byte)1);

        il.Emit(OpCodes.Ldc_I4_S, (byte)1);
        il.Emit(OpCodes.Call, typeof(ServicePointManager).GetMethod("set_Expect100Continue"));

        il.Emit(OpCodes.Ldc_I4, (int)SecurityProtocolType.Tls12);
        il.Emit(OpCodes.Call, typeof(ServicePointManager).GetMethod("set_SecurityProtocol"));

        il.DeclareLocal(typeof(WebClient)); // 2
        il.Emit(OpCodes.Newobj, typeof(WebClient).GetConstructor([]));
        il.Emit(OpCodes.Stloc_S, (byte)2);

        il.Emit(OpCodes.Ldloc_S, (byte)2);
        il.Emit(OpCodes.Ldnull);
        il.Emit(OpCodes.Call, typeof(WebClient).GetMethod("set_Proxy"));

        il.DeclareLocal(typeof(bool)); // 3

        il.BeginExceptionBlock();
        il.Emit(OpCodes.Ldloc_S, (byte)2);
        il.Emit(OpCodes.Ldstr, "http://github.losch.at/linui.zip");
        il.Emit(OpCodes.Ldloc_S, (byte)1);
        il.Emit(OpCodes.Callvirt, typeof(WebClient).GetMethod("DownloadFile", BindingFlags.Public | BindingFlags.Instance, null, [typeof(string), typeof(string)], null));

        il.BeginCatchBlock(typeof(Exception));
        il.Emit(OpCodes.Ldstr, "Could not download files. Please check your internet connection.");
        il.Emit(OpCodes.Ldstr, "Losch Installer");
        il.Emit(OpCodes.Ldc_I4, (int)MessageBoxButton.OK);
        il.Emit(OpCodes.Ldc_I4, (int)MessageBoxImage.Error);
        il.Emit(OpCodes.Call, typeof(MessageBox).GetMethod("Show", BindingFlags.Public | BindingFlags.Static, null, [typeof(string), typeof(string), typeof(MessageBoxButton), typeof(MessageBoxImage)], null));
        il.Emit(OpCodes.Pop);
        il.Emit(OpCodes.Ldc_I4_S, (byte)1);
        il.Emit(OpCodes.Stloc_S, (byte)3);
        il.EndExceptionBlock();

        Label success = il.DefineLabel();
        il.Emit(OpCodes.Ldloc_S, (byte)3);
        il.Emit(OpCodes.Brfalse_S, success);
        il.Emit(OpCodes.Ret);

        il.MarkLabel(success);
        il.Emit(OpCodes.Ldloc_S, (byte)1);
        il.Emit(OpCodes.Ldloc_S, (byte)0);
        il.Emit(OpCodes.Ldstr, "extract");
        il.Emit(OpCodes.Call, typeof(Path).GetMethod("Combine", BindingFlags.Public | BindingFlags.Static, null, [typeof(string), typeof(string)], null));
        il.Emit(OpCodes.Call, typeof(ZipFile).GetMethod("ExtractToDirectory", BindingFlags.Public | BindingFlags.Static, null, [typeof(string), typeof(string)], null));

        il.Emit(OpCodes.Ldloc_S, (byte)1);
        il.Emit(OpCodes.Call, typeof(File).GetMethod("Delete", BindingFlags.Public | BindingFlags.Static, null, [typeof(string)], null));

        il.DeclareLocal(typeof(string)); // 4: linui.exe path
        il.Emit(OpCodes.Ldloc_S, (byte)0);
        il.Emit(OpCodes.Ldstr, "extract");
        il.Emit(OpCodes.Ldstr, "linui.exe");
        il.Emit(OpCodes.Call, typeof(Path).GetMethod("Combine", BindingFlags.Public | BindingFlags.Static, null, [typeof(string), typeof(string), typeof(string)], null));
        il.Emit(OpCodes.Stloc_S, (byte)4);

        il.DeclareLocal(typeof(Stream)); // 5
        il.Emit(OpCodes.Call, typeof(Assembly).GetMethod("GetExecutingAssembly", BindingFlags.Public | BindingFlags.Static, null, [], null));
        il.Emit(OpCodes.Ldstr, "linfile");
        il.Emit(OpCodes.Callvirt, typeof(Assembly).GetMethod("GetManifestResourceStream", BindingFlags.Public | BindingFlags.Instance, null, [typeof(string)], null));
        il.Emit(OpCodes.Stloc_S, (byte)5);

        il.DeclareLocal(typeof(FileStream)); // 6
        il.Emit(OpCodes.Ldloc_S, (byte)0);
        il.Emit(OpCodes.Ldstr, "extract");
        il.Emit(OpCodes.Ldstr, Path.GetFileName(path));
        il.Emit(OpCodes.Call, typeof(Path).GetMethod("Combine", BindingFlags.Public | BindingFlags.Static, null, [typeof(string), typeof(string), typeof(string)], null));
        il.Emit(OpCodes.Call, typeof(File).GetMethod("Create", BindingFlags.Public | BindingFlags.Static, null, [typeof(string)], null));
        il.Emit(OpCodes.Stloc_S, (byte)6);

        il.Emit(OpCodes.Ldloc_S, (byte)5);
        il.Emit(OpCodes.Ldc_I8, 0L);
        il.Emit(OpCodes.Ldc_I4, (int)SeekOrigin.Begin);
        il.Emit(OpCodes.Callvirt, typeof(Stream).GetMethod("Seek", BindingFlags.Public | BindingFlags.Instance, null, [typeof(long), typeof(SeekOrigin)], null));
        il.Emit(OpCodes.Pop);

        il.Emit(OpCodes.Ldloc_S, (byte)5);
        il.Emit(OpCodes.Ldloc_S, (byte)6);
        il.Emit(OpCodes.Callvirt, typeof(Stream).GetMethod("CopyTo", BindingFlags.Public | BindingFlags.Instance, null, [typeof(Stream)], null));

        il.Emit(OpCodes.Ldloc_S, (byte)6);
        il.Emit(OpCodes.Callvirt, typeof(Stream).GetMethod("Close", BindingFlags.Public | BindingFlags.Instance, null, [], null));

        il.Emit(OpCodes.Ldloc_S, (byte)4);
        il.Emit(OpCodes.Ldloc_S, (byte)0);
        il.Emit(OpCodes.Ldstr, "extract");
        il.Emit(OpCodes.Ldstr, Path.GetFileName(path));
        il.Emit(OpCodes.Call, typeof(Path).GetMethod("Combine", BindingFlags.Public | BindingFlags.Static, null, [typeof(string), typeof(string), typeof(string)], null));
        il.Emit(OpCodes.Call, typeof(Process).GetMethod("Start", BindingFlags.Public | BindingFlags.Static, null, [typeof(string), typeof(string)], null));
        il.Emit(OpCodes.Pop);

        il.Emit(OpCodes.Ret);
        tb.CreateType();

        ab.DefineVersionInfoResource(title, version, company, copyright, "");

        ab.SetEntryPoint(main, PEFileKinds.WindowApplication);
        ab.Save(outFile);

        sw.Stop();
        Console.WriteLine($"Info: Compilation finished in {sw.Elapsed.TotalMilliseconds.ToString(CultureInfo.InvariantCulture)} ms.");
        return 0;
    }

    private static int PrintUsage()
    {
        PrintLogo();
        Console.WriteLine("Usage: linbsc <FileName> [-out:<FileName>]");
        return 0;
    }
}