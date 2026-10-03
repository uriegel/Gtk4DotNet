using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class GBytes : BaseHandle
{
    public GBytes(string str)
    {
        var utf8Bytes = System.Text.Encoding.UTF8.GetBytes(str);

        var unmanagedPtr = Marshal.AllocHGlobal(utf8Bytes.Length);
        Marshal.Copy(utf8Bytes, 0, unmanagedPtr, utf8Bytes.Length);
        SetInternalHandle(New(unmanagedPtr, utf8Bytes.Length));
        Marshal.FreeHGlobal(unmanagedPtr);
    }

    public GBytes(byte[] data, long size) => SetInternalHandle(New(data, size));

    public GBytes(byte[] data) : this(data, data.Length) {} 

    public GBytes(nint data, long size) => SetInternalHandle(New(data, size));

    public nint GetData(out long size) => GetData(this, out size);
    
    protected override bool ReleaseHandle()
    {
        Unref(handle);
        return true;
    }

    [DllImport(Libs.LibGtk, EntryPoint="g_bytes_get_data", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetData(GBytes bytes, out long size);


    [DllImport(Libs.LibGtk, EntryPoint = "g_bytes_new", CallingConvention = CallingConvention.Cdecl)]
    extern static nint New(byte[] data, long size);
    
    [DllImport(Libs.LibGtk, EntryPoint = "g_bytes_new", CallingConvention = CallingConvention.Cdecl)]
    extern static nint New(nint data, long size);

    [DllImport(Libs.LibGtk, EntryPoint = "g_bytes_unref", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void Unref(nint bytes);
}
