using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class StyleContext : FloatingObject
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_style_context_add_provider_for_display", CallingConvention = CallingConvention.Cdecl)]
    public extern static void AddProviderForDisplay(Display display, CssProvider provider, StyleProviderPriority priority);

    public GtkRgba GetColor()
    {
        var color = new GtkRgba();
        GetColor(this, ref color);
        return color;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_style_context_get_color", CallingConvention = CallingConvention.Cdecl)]
    extern static void GetColor(StyleContext styleContext, ref GtkRgba color);
}

