using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class ListItem
{
    [DllImport(Globals.LibGtk, EntryPoint="gtk_list_item_set_child", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetChild(IntPtr listItem, IntPtr child);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_list_item_get_child", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetChild(IntPtr listItem);

    [DllImport(Globals.LibGtk, EntryPoint="gtk_list_item_get_item", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetItem(IntPtr listItem);
}