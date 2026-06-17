using System.Runtime.InteropServices;
using Gtk4DotNet.Extensions;

namespace Gtk4DotNet;

public class Drive : GObject
{
    public string? Name { get => _GetName(this).PtrToString(true); }

    public string? UnixDevice { get => _GetIdentifier(this, "unix-device").PtrToString(true); }
    

    [DllImport(Libs.LibGio, EntryPoint = "g_drive_get_name", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetName(Drive drive);

    [DllImport(Libs.LibGio, EntryPoint = "g_drive_get_identifier", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetIdentifier(Drive drive, string kind);
}
