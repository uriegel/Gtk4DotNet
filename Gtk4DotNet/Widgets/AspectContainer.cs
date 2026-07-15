using System.Reflection;
using System.Runtime.InteropServices;
using CsTools;
using CsTools.Extensions;

namespace Gtk4DotNet;

public class AspectContainer : Widget
{
    public static nint GetObjectType() => getType();
    public double AspectRatio { set => setAspectRatio(GetInternalHandle(), value); }

    public AspectContainer() : base() { }

    public AspectContainer(Builder builder, string? name = null) : base(builder, name) { }

    public AspectContainer(Builder builder, string name, Action<nint> replaceParent)
        : base(builder, name, replaceParent) { }

    static AspectContainer()
    {
        string targetFileName = "";
        try
        {
            targetFileName =
                Environment
                    .GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
                    .AppendPath(@$"{"de.uriegel.gtk4dotnet"}")
                    .EnsureDirectoryExists()
                    .AppendPath("libgtk4dotnet.so");
            using var targetFile = File.Create(targetFileName);
            Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream("libgtk4dotnet")
                ?.CopyTo(targetFile);
        }
        catch { }
        lib = NativeLibrary.Load(targetFileName);
        getType = Marshal.GetDelegateForFunctionPointer<GetTypeDelegate>(NativeLibrary.GetExport(lib, "tgtk_aspect_container_get_type"));
        setAspectRatio = Marshal.GetDelegateForFunctionPointer<SetAspectRatioDelegate>(NativeLibrary.GetExport(lib, "tgtk_aspect_container_set_aspect_ratio"));
    }

    static readonly GetTypeDelegate getType;
    static readonly SetAspectRatioDelegate setAspectRatio;

    static readonly nint lib;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate nint GetTypeDelegate();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate void SetAspectRatioDelegate(nint p, double valaue);

    // [DllImport("libtgtk4dotnet.so", CallingConvention = CallingConvention.Cdecl)]
    // static extern nint tgtk_aspect_container_new();

    // [DllImport("libtgtk4dotnet.so", CallingConvention = CallingConvention.Cdecl)]
    // static extern void tgtk_aspect_container_set_child(nint ac, nint widget);

    // [DllImport("libgtk-4.so.1", EntryPoint = "g_object_set", CallingConvention = CallingConvention.Cdecl)]
    // extern static void SetValue(nint obj, string name, double value, nint end);
}
