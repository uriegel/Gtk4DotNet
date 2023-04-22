using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class GestureClick
{
    [DllImport(Globals.LibGtk, EntryPoint="gtk_gesture_click_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New();
}

