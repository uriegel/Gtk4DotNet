using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class Display : FloatingObject
{
    public static Display GetDefault() =>  _GetDefault();
    
    // [DllImport(Libs.LibGtk, EntryPoint = "gtk_icon_theme_get_for_display", CallingConvention = CallingConvention.Cdecl)]
    // public extern static IconThemeHandle GetIconTheme(Display display);

    // [DllImport(Libs.LibGtk, EntryPoint = "gdk_display_get_default_seat", CallingConvention = CallingConvention.Cdecl)]
    // public extern static GdkSeatHandle GetDefaultSeat(Display display);

    [DllImport(Libs.LibGtk, EntryPoint = "gdk_display_get_default", CallingConvention = CallingConvention.Cdecl)]
    extern static Display _GetDefault();
}
