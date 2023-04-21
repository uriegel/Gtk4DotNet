using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class PropertyAction
{
    [DllImport(Globals.LibGtk, EntryPoint="g_property_action_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New(string name, IntPtr obj, string propertyName);
}

