using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public static class Entry
{
    [DllImport(Globals.LibGtk, EntryPoint="gtk_entry_set_text", CallingConvention = CallingConvention.Cdecl)]
    public extern static void EntrySetText(this IntPtr headerBar, string text);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_entry_get_text", CallingConvention = CallingConvention.Cdecl)]
    extern static IntPtr EntryGetText(this IntPtr headerBar);
}
