using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public static class Box
{
    [DllImport(Globals.LibGtk, EntryPoint="gtk_box_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New(Orientation orientation, int spacing = 0);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_box_append", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Append(this IntPtr box, IntPtr widget);
}

