using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class Image
{
    [DllImport(Globals.LibGtk, EntryPoint="gtk_image_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New();

    [DllImport(Globals.LibGtk, EntryPoint="gtk_image_set_from_gicon", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetFromGIcon(IntPtr image, IntPtr gicon);
}