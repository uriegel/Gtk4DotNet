using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;
public static class GError
{
    [DllImport(Globals.LibGtk, EntryPoint = "g_error_free", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Free(IntPtr handle);
}


