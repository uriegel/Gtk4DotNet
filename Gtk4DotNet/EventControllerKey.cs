using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public static class EventControllerKey
{
    [DllImport(Globals.LibGtk, EntryPoint = "gtk_event_controller_key_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New();

    public delegate bool KeyPressedDelegate(IntPtr eventControllerKey, int keyval, int keycode, GdkModifierType state, IntPtr zero);
}