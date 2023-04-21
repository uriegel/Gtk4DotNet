using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class Listbox
{
    [DllImport(Globals.LibGtk, EntryPoint="gtk_list_box_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New();

    [DllImport(Globals.LibGtk, EntryPoint="gtk_list_box_insert", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Insert(IntPtr listbox, IntPtr widget, int position = -1);
    
    [DllImport(Globals.LibGtk, EntryPoint="gtk_list_box_prepend", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Prepend(IntPtr listbox, IntPtr widget);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_list_box_remove", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Remove(IntPtr listbox, IntPtr child);
}

