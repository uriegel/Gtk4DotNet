using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class GBytes
{
    [DllImport(Libs.LibGtk, EntryPoint="g_bytes_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static BytesHandle New(IntPtr data, long size);

    [DllImport(Libs.LibGtk, EntryPoint="g_bytes_get_data", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetData(this BytesHandle bytes, out long size);

    [DllImport(Libs.LibGtk, EntryPoint="g_bytes_unref", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void Unref(this IntPtr bytes);
}

