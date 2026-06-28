using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class Display : GObject
{
    public static Display GetDefault()
    {
        var res = _GetDefault();
        res.WeakCopy = true;
        return res;
    }

    public IconTheme GetIconTheme ()
    {
        var res = GetIconTheme(this);
        res.WeakCopy = true;
        return res;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_icon_theme_get_for_display", CallingConvention = CallingConvention.Cdecl)]
    extern static IconTheme GetIconTheme(Display display);

    // [DllImport(Libs.LibGtk, EntryPoint = "gdk_display_get_default_seat", CallingConvention = CallingConvention.Cdecl)]
    // public extern static GdkSeatHandle GetDefaultSeat(Display display);

    [DllImport(Libs.LibGtk, EntryPoint = "gdk_display_get_default", CallingConvention = CallingConvention.Cdecl)]
    extern static Display _GetDefault();
}
