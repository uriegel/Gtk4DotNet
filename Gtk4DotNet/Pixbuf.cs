using System;
using System.IO;
using System.Runtime.InteropServices;

namespace GtkDotNet;

#nullable enable

public class Pixbuf
{
    public static IntPtr NewFromFile(string fileName) => NewFromFile(fileName, IntPtr.Zero);
    [DllImport(Globals.LibGtk, EntryPoint="gdk_pixbuf_new_from_file", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr NewFromFile(string fileName, IntPtr err);

    public static bool SaveJpg(IntPtr pixbuf, string filename)
    {
        var error = IntPtr.Zero;
        return Save(pixbuf, filename, "jpeg", ref error, "quality", "50", IntPtr.Zero);
    }
        
    public static Stream? SaveJpgToBuffer(IntPtr pixbuf)
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

    public static IntPtr NewFromResource(string resourcePath) => NewFromResource( resourcePath, IntPtr.Zero);
    [DllImport(Globals.LibGtk, EntryPoint="gdk_pixbuf_new_from_resource", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr NewFromResource(string resourcePath, IntPtr err);

    [DllImport(Globals.LibGtk, EntryPoint="gdk_pixbuf_scale_simple", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr Scale(IntPtr source, int width, int height, Interpolation interpolation);

    [DllImport(Globals.LibGtk, EntryPoint="gdk_pixbuf_save", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool Save(IntPtr pixbuf, string filename, string type, ref IntPtr error, string k, string v, IntPtr nil);

    [DllImport(Globals.LibGtk, EntryPoint="gdk_pixbuf_save_to_buffer", CallingConvention = CallingConvention.Cdecl)]
    extern static bool SaveToBuffer(IntPtr pixbuf, out IntPtr buffer, out int bufferSize, string type, ref IntPtr error, string k, string v, IntPtr nil);
    [DllImport(Globals.LibGtk, EntryPoint="gdk_pixbuf_get_file_info", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetFileInfo(string filename, out int width, out int height);
}

