using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public static class GtkSettings
{
    [DllImport(Globals.LibGtk, EntryPoint = "gtk_settings_get_default", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetDefault();
} 

