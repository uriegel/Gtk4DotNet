using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class Grid
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_grid_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static GridHandle New();

    public static GridHandle Attach(this GridHandle grid, WidgetHandle widget, int column, int row, int columnSpan, int rowSpan)
        => grid.SideEffect(g => g._Attach(widget, column, row, columnSpan, rowSpan));

    public static GridHandle RowSpacing(this GridHandle grid, int spacing)
        => grid.SideEffect(g => g.SetRowSpacing(spacing));

    public static GridHandle ColumnSpacing(this GridHandle grid, int spacing)
        => grid.SideEffect(g => g.SetColumnSpacing(spacing));

    [DllImport(Libs.LibGtk, EntryPoint="gtk_grid_attach", CallingConvention = CallingConvention.Cdecl)]
    extern static void _Attach(this GridHandle grid, WidgetHandle widget, int column, int row, int columnSpan, int rowSpan);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_grid_set_row_spacing", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetRowSpacing(this GridHandle grid, int spacing);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_grid_set_column_spacing", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetColumnSpacing(this GridHandle grid, int spacing);

}

