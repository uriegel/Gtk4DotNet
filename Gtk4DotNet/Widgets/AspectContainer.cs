using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class AspectContainer : Widget
{
    public static nint GetObjectType() => getType();
    static AspectContainer()
    {
        lib = NativeLibrary.Load("/mnt/Home/Projekte/Gtk4DotNet/C-Code/gtk4dotnet/libtgtk4dotnet.so");
        getType = Marshal.GetDelegateForFunctionPointer<GetTypeDelegate>(NativeLibrary.GetExport(lib,"tgtk_aspect_container_get_type"));
    }

    static GetTypeDelegate getType;
    
    static readonly nint lib;

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    delegate nint GetTypeDelegate();

    // [DllImport("libtgtk4dotnet.so", CallingConvention = CallingConvention.Cdecl)]
    // static extern nint tgtk_aspect_container_new();

    // [DllImport("libtgtk4dotnet.so", CallingConvention = CallingConvention.Cdecl)]
    // static extern void tgtk_aspect_container_set_child(nint ac, nint widget);

    // [DllImport("libgtk-4.so.1", EntryPoint = "g_object_set", CallingConvention = CallingConvention.Cdecl)]
    // extern static void SetValue(nint obj, string name, double value, nint end);
}
