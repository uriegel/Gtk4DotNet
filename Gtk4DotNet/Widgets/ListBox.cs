using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class ListBox : Widget
{
    public SelectionMode SelectionMode
    {
        get => GetSelectionMode(this);
        set => SetSelectionMode(this, value);
    }
    public static ListBox New()
    {
        var listbox = _New();
        listbox.CheckDiagnostics();
        return listbox;
    }

    public void RemoveAll() => RemoveAll(this);
    public void Insert(Widget widget, int position = -1) => Insert(this, widget, position);
    public void Prepend(Widget widget) => Prepend(this, widget);
    public void Append(Widget widget) => Append(this, widget);
    public void Remove(Widget widget) => Remove(this, widget);

    public ListBox() : base() { }
    
    public ListBox(Builder builder, string? name = null) : base(builder, name) { }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_box_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ListBox _New();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_box_remove_all", CallingConvention = CallingConvention.Cdecl)]
    extern static void RemoveAll(ListBox listbox);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_list_box_insert", CallingConvention = CallingConvention.Cdecl)]
    extern static void Insert(ListBox listbox, Widget widget, int position);
    
    [DllImport(Libs.LibGtk, EntryPoint="gtk_list_box_prepend", CallingConvention = CallingConvention.Cdecl)]
    extern static void Prepend(ListBox listbox, Widget widget);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_list_box_append", CallingConvention = CallingConvention.Cdecl)]
    extern static void Append(ListBox listbox, Widget widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_box_remove", CallingConvention = CallingConvention.Cdecl)]
    extern static void Remove(ListBox listbox, Widget widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_box_set_selection_mode", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetSelectionMode(ListBox listbox, SelectionMode selectionMode);    

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_box_get_selection_mode", CallingConvention = CallingConvention.Cdecl)]
    extern static SelectionMode GetSelectionMode(ListBox listbox);    
}
