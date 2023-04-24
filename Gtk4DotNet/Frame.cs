using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public static class Frame
{
    [DllImport(Globals.LibGtk, EntryPoint="gtk_frame_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New(string label = null);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_frame_set_child", CallingConvention = CallingConvention.Cdecl)]
    public extern static void FrameSetChild(this IntPtr frame, IntPtr widget);
}

