using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public static class GestureSingle
{
    [DllImport(Globals.LibGtk, EntryPoint="gtk_gesture_single_set_button", CallingConvention = CallingConvention.Cdecl)]
    public extern static void GestureSingleSetButton(this IntPtr gesture, MouseButton button);
}

