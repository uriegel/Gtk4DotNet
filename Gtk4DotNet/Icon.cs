using System.Runtime.InteropServices;
using GtkDotNet.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Icon
{
    public static string[] Names(this IconHandle icon)
        => _IconToString(icon).PtrToString(true)?.Split(' ') ?? [];

    [DllImport(Libs.LibGtk, EntryPoint = "g_icon_to_string", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _IconToString(IconHandle icon);
}