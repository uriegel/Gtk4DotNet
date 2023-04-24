using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public static class Actionable
{
    [DllImport(Globals.LibGtk, EntryPoint = "gtk_actionable_set_action_name", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetActionName(this IntPtr actionable, string actionName);
}

