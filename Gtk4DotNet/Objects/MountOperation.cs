using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class MountOperation : GObject
{
    public static MountOperation New()
    {
        var op = _New();
        op.CheckDiagnostics();
        return op;
    } 
    [DllImport(Libs.LibGio, EntryPoint = "g_mount_operation_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static MountOperation _New();
}
