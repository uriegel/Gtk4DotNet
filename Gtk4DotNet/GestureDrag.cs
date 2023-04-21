using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class GestureDrag
{
    [DllImport(Globals.LibGtk, EntryPoint="gtk_gesture_drag_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New();
}

