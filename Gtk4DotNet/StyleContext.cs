using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public class StyleContext
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_style_context_add_provider_for_display", CallingConvention = CallingConvention.Cdecl)]
    public extern static void AddProviderForDisplay(DisplayHandle display, CssProviderHandle provider, StyleProviderPriority priority);
}

