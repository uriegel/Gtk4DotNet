using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class GFileInfo
{
    public static string GetDisplayName(IntPtr info)
        => Marshal.PtrToStringUTF8(_GetDisplayName(info));

    [DllImport(Globals.LibGtk, EntryPoint = "g_file_info_get_icon", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetIcon(IntPtr info);

    [DllImport(Globals.LibGtk, EntryPoint = "g_file_info_get_file_type", CallingConvention = CallingConvention.Cdecl)]
    public extern static FileType GetFileType(IntPtr info);

    [DllImport(Globals.LibGtk, EntryPoint = "g_file_info_get_is_hidden", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool IsHidden(IntPtr info);

    [DllImport(Globals.LibGtk, EntryPoint = "g_file_info_get_display_name", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr _GetDisplayName(IntPtr info);
}
