using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class GBytes
{
    [DllImport(Globals.LibGtk, EntryPoint="g_bytes_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New(IntPtr data, long size);

    [DllImport(Globals.LibGtk, EntryPoint="g_bytes_unref", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr Unref(IntPtr data);

    [DllImport(Globals.LibGtk, EntryPoint="g_bytes_get_data", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetData(IntPtr data, out long size);
}

