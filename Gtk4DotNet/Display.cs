using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class Display
{
    [DllImport(Globals.LibGtk, EntryPoint = "gdk_display_get_default", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetDefault();

    [DllImport(Globals.LibGtk, EntryPoint = "gdk_display_get_default_screen", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetDefaultScreen(IntPtr display);

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_icon_theme_get_for_display", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr IconThemeForDisplay(IntPtr display);
    
}

