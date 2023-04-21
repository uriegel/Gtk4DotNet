using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class GestureSingle
{
    [DllImport(Globals.LibGtk, EntryPoint="gtk_gesture_single_set_button", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetButton(IntPtr gesture, MouseButton button);
}

