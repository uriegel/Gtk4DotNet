using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class SignalListItemFactory
{
    [DllImport(Globals.LibGtk, EntryPoint="gtk_signal_list_item_factory_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New();

    public delegate void Delegate(IntPtr factory, IntPtr listItem, IntPtr nill);
}

