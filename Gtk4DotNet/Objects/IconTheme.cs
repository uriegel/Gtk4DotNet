using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class IconTheme : FloatingObject
{
    // TODO check options
    // TODO check fallbacks
    // TODO check symbolics
    public IconPaintable LookupIcon(string iconName, int size, IconLookupFlags flags = IconLookupFlags.ForceRegular, int scale = 1)
        => LookupIcon(this, iconName, 0, size, scale, 1, flags);
        // Don't call .CheckDiagnostics(); because these icons are cached

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_icon_theme_lookup_icon", CallingConvention = CallingConvention.Cdecl)]
    extern static IconPaintable LookupIcon(IconTheme iconTheme, string iconName, IntPtr nil, int size, int scale, int textDirection, IconLookupFlags flags);
}

