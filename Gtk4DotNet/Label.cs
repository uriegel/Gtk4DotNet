using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public static class Label
{
    [DllImport(Globals.LibGtk, EntryPoint="gtk_label_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New(string text);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_label_set_label", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetLabel(this IntPtr label, string text);
    
    [DllImport(Globals.LibGtk, EntryPoint="gtk_label_set_selectable", CallingConvention = CallingConvention.Cdecl)]
    public extern static void LabelSetSelectable(this IntPtr label, bool selectable);
}


