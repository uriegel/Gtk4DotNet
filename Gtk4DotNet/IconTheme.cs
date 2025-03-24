using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class IconTheme
{
    // [DllImport(Libs.LibGtk, EntryPoint = "gtk_icon_theme_lookup_icon", CallingConvention = CallingConvention.Cdecl)]
    // public extern static IntPtr LookupIcon(this IconThemeHandle iconTheme, string iconName, IntPtr nil, int size, int scale, TextDirection textDirection, IconLookup lookup);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_icon_theme_add_search_path", CallingConvention = CallingConvention.Cdecl)]
    public extern static void AddSearchPath(this IconThemeHandle iconTheme, string path);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_icon_theme_has_icon", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool HasIcon(this IconThemeHandle iconTheme, string path);
    
}


