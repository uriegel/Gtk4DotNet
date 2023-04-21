using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class ActionMap
{
    [DllImport(Globals.LibGtk, EntryPoint = "g_action_map_add_action", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr AddAction(IntPtr actionMap, IntPtr action);
}

