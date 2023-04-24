using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public static class ActionMap
{
    [DllImport(Globals.LibGtk, EntryPoint = "g_action_map_add_action", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr AddAction(this IntPtr actionMap, IntPtr action);
}

