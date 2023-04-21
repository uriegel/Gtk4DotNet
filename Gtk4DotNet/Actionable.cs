using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class Actionable
{
    [DllImport(Globals.LibGtk, EntryPoint = "gtk_actionable_set_action_name", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetActionName(IntPtr actionable, string actionName);
}

