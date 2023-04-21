using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class Native
{
    [DllImport(Globals.LibGtk, EntryPoint="gtk_native_get_surface", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetSurface(IntPtr surface);
}

