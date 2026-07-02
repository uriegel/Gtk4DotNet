using System.Runtime.InteropServices;
using Gtk4DotNet.Extensions;

namespace Gtk4DotNet;


public class ColumnViewColumn : GObject
{
    public string Title
    {
        get => GetTitle(this).PtrToString(false) ?? "";
        set => SetTitle(this, value);
    }

    public static ColumnViewColumn New(string title, ListItemFactory factory)
    {
        var res = _New(title, factory);
        res.CheckDiagnostics();
        factory.AutoDestroyed = true;
        return res;
    }

    public ColumnViewColumn Expand()
    {
        SetExpand(this, true);
        return this;
    }

    internal ColumnViewColumn() { }

    public void SetSorter(Sorter sorter) => SetSorter(this, sorter);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_column_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ColumnViewColumn _New(string title, ListItemFactory factory);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_column_set_expand", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetExpand(ColumnViewColumn column, bool expand);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_column_set_sorter", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetSorter(ColumnViewColumn column, Sorter sorter);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_column_get_title", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetTitle(ColumnViewColumn column);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_column_set_title", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetTitle(ColumnViewColumn column, string title);
}