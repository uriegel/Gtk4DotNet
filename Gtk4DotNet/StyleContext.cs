using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class StyleContext
{
    [DllImport(Globals.LibGtk, EntryPoint = "gtk_style_context_add_provider", CallingConvention = CallingConvention.Cdecl)]
    public extern static void AddProvider(IntPtr styleContext, IntPtr provider, StyleProviderPriority priority);

    [DllImport(Globals.LibGtk, EntryPoint = "gtk_style_context_add_provider_for_display", CallingConvention = CallingConvention.Cdecl)]
    public extern static void AddProviderForDisplay(IntPtr display, IntPtr provider, StyleProviderPriority priority);
    
}

