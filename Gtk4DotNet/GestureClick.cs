using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public static class GestureClick
{
    [DllImport(Globals.LibGtk, EntryPoint="gtk_gesture_click_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New();
}

