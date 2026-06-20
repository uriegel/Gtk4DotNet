using System.Runtime.InteropServices;
using CsTools.Extensions;

namespace Gtk4DotNet;

public class GBytes : BaseHandle
{
    public static GBytes New(string str)
    {
        var utf8Bytes = System.Text.Encoding.UTF8.GetBytes(str);

        var unmanagedPtr = Marshal.AllocHGlobal(utf8Bytes.Length);
        Marshal.Copy(utf8Bytes, 0, unmanagedPtr, utf8Bytes.Length);
        var gBytes = New(unmanagedPtr, utf8Bytes.Length);
        Marshal.FreeHGlobal(unmanagedPtr);
        return gBytes;
    }

    public static GBytes New(byte[] data) => New(data, data.Length);

    [DllImport(Libs.LibGtk, EntryPoint = "g_bytes_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static GBytes New(IntPtr data, long size);

    public nint GetData(out long size) => GetData(this, out size);
    
    public GBytes() : base() { }

    protected override bool ReleaseHandle()
    {
        Unref(handle);
        return true;
    }

    [DllImport(Libs.LibGtk, EntryPoint="g_bytes_get_data", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr GetData(GBytes bytes, out long size);

    [DllImport(Libs.LibGtk, EntryPoint="g_bytes_new", CallingConvention = CallingConvention.Cdecl)]
    extern static GBytes New(string str, long size);
    
    [DllImport(Libs.LibGtk, EntryPoint="g_bytes_new", CallingConvention = CallingConvention.Cdecl)]
    extern static GBytes New(byte[] data, long size);

    [DllImport(Libs.LibGtk, EntryPoint="g_bytes_unref", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void Unref(nint bytes);
}
