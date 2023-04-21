using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class IconTheme
{
    [DllImport(Globals.LibGtk, EntryPoint = "gtk_icon_theme_lookup_icon", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr LookupIcon(IntPtr iconTheme, string iconName, IntPtr nil, int size, int scale, TextDirection textDirection, IconLookup lookup);

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_icon_theme_add_search_path", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr AddSearchPath(IntPtr iconTheme, string path);

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_icon_theme_has_icon", CallingConvention = CallingConvention.Cdecl)]
    public extern static int HasIcon(IntPtr iconTheme, string path);
    
}


