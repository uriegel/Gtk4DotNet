using System.Runtime.InteropServices;
using Gtk4DotNet;

public class ColumnView : Widget
{
    public bool Rubberband
    {
        get => GetEnableRubberband(this);
        set => SetEnableRubberband(this, value);
    }


    public void SetModel(SelectionModel selectionModel) => SetModel(this, selectionModel);

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
        

    readonly List<ColumnViewColumn> cols = [];

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_set_model", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetModel(ColumnView columnView, SelectionModel selectionModel);

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
}
