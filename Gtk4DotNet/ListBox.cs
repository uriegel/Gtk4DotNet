using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class ListBox
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_box_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static ListBoxHandle New();

    [DllImport(Libs.LibGtk, EntryPoint="gtk_list_box_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle Type();        

    public static ListBoxHandle SelectionMode(this ListBoxHandle listbox, SelectionMode selectionMode)
        => listbox.SideEffect(l => l.SetSelectionMode(selectionMode));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_box_remove_all", CallingConvention = CallingConvention.Cdecl)]
    public extern static void RemoveAll(this ListBoxHandle listbox);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_list_box_insert", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Insert(this ListBoxHandle listbox, WidgetHandle widget, int position = -1);
    
    [DllImport(Libs.LibGtk, EntryPoint="gtk_list_box_prepend", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Prepend(this ListBoxHandle listbox, WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_list_box_remove", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Remove(this ListBoxHandle listbox, WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_box_set_selection_mode", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetSelectionMode(this ListBoxHandle listbox, SelectionMode selectionMode);
}