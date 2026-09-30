using System.Runtime.InteropServices;
using CsTools.Extensions;

namespace Gtk4DotNet;

public class Grid : Widget
{
    public int RowSpacing
    {
        get => GetRowSpacing(this);
        set => SetRowSpacing(this, value);
    }
    public int ColumnSpacing
    {
        get => GetColumnSpacing(this);
        set => SetColumnSpacing(this, value);
    }

    public Grid() : base()
    {
        var handle = New();
        SetInternalHandle(handle);
        CheckDiagnostics();
    }
    public Grid Attach(Widget widget, int column, int row, int columnSpan, int rowSpan)
        => this.SideEffect(g => Attach(this, widget, column, row, columnSpan, rowSpan));

    public Grid SetRowSpacing(int spacing)
        => this.SideEffect(g => SetRowSpacing(this, spacing));

    public Grid SetColumnSpacing(int spacing)
        => this.SideEffect(g => SetColumnSpacing(this, spacing));

    public Grid(Builder builder, string? name = null) : base(builder, name) { }

    public Grid(Builder builder, string name, Action<nint> replaceParent)
        : base(builder, name, replaceParent) { }

    [DllImport(Libs.LibGtk, EntryPoint="gtk_grid_new", CallingConvention = CallingConvention.Cdecl)]
    extern static nint New();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_grid_attach", CallingConvention = CallingConvention.Cdecl)]
    extern static void Attach(Grid grid, Widget widget, int column, int row, int columnSpan, int rowSpan);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_grid_get_row_spacing", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetRowSpacing(Grid grid);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_grid_set_row_spacing", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetRowSpacing(Grid grid, int spacing);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_grid_get_column_spacing", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetColumnSpacing(Grid grid);

    [DllImport(Libs.LibGtk, EntryPoint="gtk_grid_set_column_spacing", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetColumnSpacing(Grid grid, int spacing);
}

