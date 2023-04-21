using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class ListView
{
    [DllImport(Globals.LibGtk, EntryPoint="gtk_list_view_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New(IntPtr selectionModel, IntPtr itemFactory);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_list_view_set_model", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetModel(IntPtr listView, IntPtr selectionModel);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_list_view_set_factory", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetFactory(IntPtr listView, IntPtr itemFactory);
}

