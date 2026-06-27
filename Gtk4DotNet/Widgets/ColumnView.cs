using System.Runtime.InteropServices;
using Gtk4DotNet;

public class ColumnView : Widget
{
    public bool Rubberband
    {
        get => GetEnableRubberband(this);
        set => SetEnableRubberband(this, value);
    }


    public void SetModel(SelectionModel? selectionModel) => SetModel(this, selectionModel!= null ? selectionModel.GetInternalHandle() : 0);

    public void AppendColumn(ColumnViewColumn column)
    {
        AppendColumn(this, column);  
        cols.Add(column);  
    }

    public void RemoveColumn(ColumnViewColumn column)
    {
        RemoveColumn(this, column);
        cols.Remove(column);
    }
    
    public void ClearColumns()
    {
        foreach (var col in cols)
        {
            RemoveColumn(this, col);
            col.Dispose();
        }
        cols.Clear();
    }

    public ColumnView(Builder builder, string? name = null) : base(builder, name)
        => OnFinalize(() =>
        {
            foreach (var col in cols)
                col.Dispose();
            cols.Clear();
        });

    public int GetFocusedItemPos()
    {
        window ??= GetAncestor<Window>();
        var row = window.GetFocus<Widget>();
        if (!IsWidgetInColumnView(row))
            return -1;
        if (!row.IsInvalid && row.GetName() == "GtkColumnViewRowWidget")
        {
            var ptr = row.GetManagedRawData(ListStore.DATA);
            return GetModel().GetRawItems().TakeWhile(n => n != ptr).Count();
        }
        else
            return -1;
    }

    public void ScrollTo(int pos, ListScrollFlags flags) =>  ScrollTo(this, pos, 0, flags, 0);

    public Sorter GetSorter() => GetSorter(this);

    public SelectionModel GetModel()
    {
        var res = GetModel(this);
        res.AutoDestroyed = true;
        return res;
    }
    
    bool IsWidgetInColumnView(Widget w)
    {
        while (true)
        {
            var p = w.GetParent();
            if (p.IsInvalid)
                return false;
            if (p.GetInternalHandle() == GetInternalHandle())
                return true;
            w = p;
        }
    }

    Window? window = null;

    readonly List<ColumnViewColumn> cols = [];

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_set_model", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetModel(ColumnView columnView, nint selectionModel);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_append_column", CallingConvention = CallingConvention.Cdecl)]
    extern static void AppendColumn(ColumnView columnView, ColumnViewColumn column);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_get_enable_rubberband", CallingConvention = CallingConvention.Cdecl)]
    extern static bool GetEnableRubberband(ColumnView columnView);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_set_enable_rubberband", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetEnableRubberband(ColumnView columnView, bool enable);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_get_columns", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetColumns(ColumnView columnView);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_remove_column", CallingConvention = CallingConvention.Cdecl)]
    extern static void RemoveColumn(ColumnView columnView, ColumnViewColumn col);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_get_sorter", CallingConvention = CallingConvention.Cdecl)]
    extern static CustomSorter GetSorter(ColumnView columnView);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_get_model", CallingConvention = CallingConvention.Cdecl)]
    extern static SelectionModel GetModel(ColumnView columnView);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_scroll_to", CallingConvention = CallingConvention.Cdecl)]
    extern static void ScrollTo(ColumnView columnView, int pos, nint nilc, ListScrollFlags flags, nint nil);
}
