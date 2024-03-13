using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class Pixbuf
{
    public static PixbufHandle NewFromFile(string filename)
        => NewFromFile(filename, IntPtr.Zero);

    public static bool SaveJpg(PixbufHandle pixbuf, string filename)
    {
        var error = IntPtr.Zero;
        return Save(pixbuf, filename, "jpeg", ref error, "quality", "50", IntPtr.Zero);
    }
        
    public static Stream? SaveJpgToBuffer(PixbufHandle pixbuf)
    {
        var error = IntPtr.Zero;
        if (SaveToBuffer(pixbuf, out var buffer, out var size, "jpeg", ref error, "quality", "50", IntPtr.Zero))
        {
            var bytes = new byte[size];
            Marshal.Copy(buffer, bytes, 0, size);
            var result = new MemoryStream(bytes);
            GObject.Free(buffer);
            return result;
        } else
            return null;
    }

    public static (int, int) GetFileInfo(this string filename)
    {
        GetFileInfo(filename, out var x, out var y);
        return (x, y);
    }

    [DllImport(Libs.LibGtk, EntryPoint="gdk_pixbuf_scale_simple", CallingConvention = CallingConvention.Cdecl)]
    public extern static PixbufHandle Scale(this PixbufHandle source, int width, int height, Interpolation interpolation);

    [DllImport(Libs.LibGtk, EntryPoint = "gdk_pixbuf_new_from_file", CallingConvention = CallingConvention.Cdecl)]
    extern static PixbufHandle NewFromFile(string filename, IntPtr _);

    [DllImport(Libs.LibGtk, EntryPoint="gdk_pixbuf_save", CallingConvention = CallingConvention.Cdecl)] 
    public extern static bool Save(PixbufHandle pixbuf, string filename, string type, ref IntPtr error, string k, string v, IntPtr nil);

    [DllImport(Libs.LibGtk, EntryPoint="gdk_pixbuf_save_to_buffer", CallingConvention = CallingConvention.Cdecl)]
    extern static bool SaveToBuffer(PixbufHandle pixbuf, out IntPtr buffer, out int bufferSize, string type, ref IntPtr error, string k, string v, IntPtr nil);    

    [DllImport(Libs.LibGtk, EntryPoint="gdk_pixbuf_get_file_info", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetFileInfo(string filename, out int width, out int height);
}