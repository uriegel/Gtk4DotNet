using System.Runtime.InteropServices;
using Gtk4DotNet;

public class ColumnViewColumn : GObject
{
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

    public void SetSorter(Sorter sorter) => SetSorter(this, sorter);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_column_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ColumnViewColumn _New(string title, ListItemFactory factory);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_column_set_expand", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetExpand(ColumnViewColumn column, bool expand);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_column_set_sorter", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetSorter(ColumnViewColumn column, Sorter sorter);
}