using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public static class ColumnView
{
    [DllImport(Globals.LibGtk, EntryPoint="gtk_column_view_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New(IntPtr selectionModel);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_column_view_set_model", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetModel(this IntPtr columnView, IntPtr selectionModel);
    
    [DllImport(Globals.LibGtk, EntryPoint="gtk_column_view_append_column", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr AppendColumn(this IntPtr columnView, IntPtr columnViewColumn);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_column_view_set_reorderable", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr SetReorderable(this IntPtr columnView, bool reorderable);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_column_view_get_sorter", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetSorter(this IntPtr columnView);
}

