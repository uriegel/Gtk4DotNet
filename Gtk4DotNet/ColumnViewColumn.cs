using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class ColumnViewColumn
{
    [DllImport(Globals.LibGtk, EntryPoint="gtk_column_view_column_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New(string title, IntPtr listItemFactory);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_column_view_column_set_resizable", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetResizable(IntPtr column, bool resizable);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_column_view_column_set_expand", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetExpand(IntPtr column, bool expand);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_column_view_column_set_sorter", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetSorter(IntPtr column, IntPtr sorter);
}
