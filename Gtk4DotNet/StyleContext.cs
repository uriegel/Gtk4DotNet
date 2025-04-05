using System.Drawing;
using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class StyleContext
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_style_context_add_provider_for_display", CallingConvention = CallingConvention.Cdecl)]
    public extern static void AddProviderForDisplay(DisplayHandle display, CssProviderHandle provider, StyleProviderPriority priority);

    public static GtkRgba GetColor(this StyleContextHandle styleContext)
    {
        var color = new GtkRgba();
        GetColor(styleContext, ref color);
        return color;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_style_context_get_color", CallingConvention = CallingConvention.Cdecl)]
    extern static void GetColor(this StyleContextHandle styleContext, ref GtkRgba color);
}

