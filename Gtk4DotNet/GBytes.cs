using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class GBytes
{
    [DllImport(Libs.LibGtk, EntryPoint="g_bytes_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static BytesHandle New(IntPtr data, long size);

    public static BytesHandle New(string str)
    {
        var utf8Bytes = System.Text.Encoding.UTF8.GetBytes(str);

        var unmanagedPtr = Marshal.AllocHGlobal(utf8Bytes.Length);
        Marshal.Copy(utf8Bytes, 0, unmanagedPtr, utf8Bytes.Length);
        var gBytes = New(unmanagedPtr, utf8Bytes.Length);
        Marshal.FreeHGlobal(unmanagedPtr);
        return gBytes;
    }


    public static BytesHandle New(byte[] data)
        => New(data, data.Length);

    [DllImport(Libs.LibGtk, EntryPoint="g_bytes_get_data", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetData(this BytesHandle bytes, out long size);

    [DllImport(Libs.LibGtk, EntryPoint="g_bytes_new", CallingConvention = CallingConvention.Cdecl)]
    extern static BytesHandle New(string str, long size);
    
    [DllImport(Libs.LibGtk, EntryPoint="g_bytes_new", CallingConvention = CallingConvention.Cdecl)]
    extern static BytesHandle New(byte[] data, long size);

    [DllImport(Libs.LibGtk, EntryPoint="g_bytes_unref", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void Unref(this IntPtr bytes);
}

