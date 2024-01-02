using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Display
{
    [DllImport(Libs.LibGtk, EntryPoint = "gdk_display_get_default", CallingConvention = CallingConvention.Cdecl)]
    public extern static DisplayHandle GetDefault();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_icon_theme_get_for_display", CallingConvention = CallingConvention.Cdecl)]
    public extern static IconThemeHandle GetIconTheme(this DisplayHandle display);
}
