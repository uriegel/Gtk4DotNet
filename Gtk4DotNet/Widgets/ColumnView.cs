using System.Net.WebSockets;
using System.Runtime.InteropServices;
using Gtk4DotNet;

public class ColumnView : Widget
{
    public bool Rubberband
    {
        get => GetEnableRubberband(this);
        set => SetEnableRubberband(this, value);
    }

    public void SetModel(SelectionModel? selectionModel)
    {
        GetModel()?.OnItemsChanged -= OnItemsChanged;
        SetModel(this, selectionModel != null ? selectionModel.GetInternalHandle() : 0);
        selectionModel?.OnItemsChanged += OnItemsChanged;
        positions = null;
    }

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
        positions ??= CreatePositions();
        var row = GetRoot<Window>()?.GetFocus<Widget>();
        if (row == null || !IsWidgetInColumnView(row))
            return -1;
        if (!row.IsInvalid && row.Name == "GtkColumnViewRowWidget")
        {
            var ptr = row.GetManagedRawData(ListStore.DATA);
            return positions?.TryGetValue(ptr, out var pos) == true ? pos : -1;
        }
        else
            return -1;
    }

    public void ScrollTo(int pos, ListScrollFlags flags) =>  ScrollTo(this, pos, 0, flags, 0);

    public Sorter GetSorter() => GetSorter(this);

    public SelectionModel? GetModel()
    {
        var m = GetModel(this);
        if (m == 0)
            return null;
        var res = new SelectionModel()
        {
            AutoDestroyed = true
        };
        res.SetInternalHandle(m);
        return res;
    }

    public int ItemsCount() => (positions ??= CreatePositions())?.Count ?? 0;

    void OnItemsChanged(int position, int removed, int added)
        => positions = null;
    
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

    Dictionary<nint, int>? CreatePositions()
        => GetModel()?.GetRawItems().Select((n, i) => (n, i)).ToDictionary();

    Dictionary<nint, int>? positions;    

    readonly List<ColumnViewColumn> cols = [];   [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_set_model", CallingConvention = CallingConvention.Cdecl)]
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
    extern static nint GetModel(ColumnView columnView);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_scroll_to", CallingConvention = CallingConvention.Cdecl)]
    extern static void ScrollTo(ColumnView columnView, int pos, nint nilc, ListScrollFlags flags, nint nil);
}
