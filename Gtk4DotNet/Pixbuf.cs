using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class Pixbuf
{
    public static IntPtr NewFromFile(string fileName) => NewFromFile(fileName, IntPtr.Zero);
    [DllImport(Globals.LibGtk, EntryPoint="gdk_pixbuf_new_from_file", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr NewFromFile(string fileName, IntPtr err);

    public static bool SaveJpg(IntPtr pixbuf, string filename)
    {
        var error = IntPtr.Zero;
        return Save(pixbuf, filename, "jpeg", ref error, IntPtr.Zero);
    }
        

    public static IntPtr NewFromResource(string resourcePath) => NewFromResource( resourcePath, IntPtr.Zero);
    [DllImport(Globals.LibGtk, EntryPoint="gdk_pixbuf_new_from_resource", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr NewFromResource(string resourcePath, IntPtr err);

    [DllImport(Globals.LibGtk, EntryPoint="gdk_pixbuf_scale_simple", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr Scale(IntPtr source, int width, int height, Interpolation interpolation);

    [DllImport(Globals.LibGtk, EntryPoint="gdk_pixbuf_save", CallingConvention = CallingConvention.Cdecl)]
    public extern static bool Save(IntPtr pixbuf, string filename, string type, ref IntPtr error, IntPtr nil);

    [DllImport(Globals.LibGtk, EntryPoint="gdk_pixbuf_get_file_info", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetFileInfo(string filename, out int width, out int height);
}

