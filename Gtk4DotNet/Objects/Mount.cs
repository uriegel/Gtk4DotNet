using System.Runtime.InteropServices;
using Gtk4DotNet.Extensions;

namespace Gtk4DotNet;

public class Mount : GObject
{
    public string? Name { get => GetName(this).PtrToString(true); }
    public string? Uuid { get => GetUuid(this).PtrToString(true); }
    public GFile GetRoot()
    {
        var res = GetRoot(this);
        // Mount a living object:
        // res.CheckDiagnostics();
        return res;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "g_mount_get_volume", CallingConvention = CallingConvention.Cdecl)]
    extern static Volume GetVolume(Mount mount);

    [DllImport(Libs.LibGtk, EntryPoint = "g_mount_get_root", CallingConvention = CallingConvention.Cdecl)]
    public extern static GFile GetRoot(Mount mount);

    [DllImport(Libs.LibGtk, EntryPoint = "g_mount_get_name", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetName(Mount mount);

    [DllImport(Libs.LibGtk, EntryPoint = "g_mount_get_uuid", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetUuid(Mount mount);
}
