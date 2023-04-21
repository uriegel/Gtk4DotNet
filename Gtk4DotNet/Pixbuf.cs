using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class Pixbuf
{
    public static IntPtr NewFromFile(string fileName) => NewFromFile(fileName, IntPtr.Zero);
    [DllImport(Globals.LibGtk, EntryPoint="gdk_pixbuf_new_from_file", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr NewFromFile(string fileName, IntPtr err);

    public static IntPtr NewFromResource(string resourcePath) => NewFromResource( resourcePath, IntPtr.Zero);
    [DllImport(Globals.LibGtk, EntryPoint="gdk_pixbuf_new_from_resource", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr NewFromResource(string resourcePath, IntPtr err);
}

