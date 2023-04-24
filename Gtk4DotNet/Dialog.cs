using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;
public static class Dialog
{
    [DllImport(Globals.LibGtk, EntryPoint="gtk_dialog_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New();

    [DllImport(Globals.LibGtk, EntryPoint="gtk_dialog_new_with_buttons", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New(string title, IntPtr parent, DialogFlags flags, string firstButton, string secondButton = null, string thirdButton = null);
}

