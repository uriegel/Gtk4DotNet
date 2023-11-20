using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

public static class Grid
{
    [DllImport(Libs.LibGtk, EntryPoint="gtk_grid_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static GridHandle New();

    public static GridHandle Attach(this GridHandle grid, WidgetHandle widget, int column, int row, int columnSpan, int rowSpan)
        => grid.SideEffect(g => g._Attach(widget, column, row, columnSpan, rowSpan));

    [DllImport(Libs.LibGtk, EntryPoint="gtk_grid_attach", CallingConvention = CallingConvention.Cdecl)]
    extern static void _Attach(this GridHandle grid, WidgetHandle widget, int column, int row, int columnSpan, int rowSpan);
}

