using System.Runtime.InteropServices;
using CsTools.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class ColumnView
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static ColumnViewHandle New(SingleSelectionHandle selectionModel);

    public static ColumnViewHandle AppendColumn(this ColumnViewHandle columnView, ColumnViewColumnHandle column)
        => columnView.SideEffect(c => c._AppendColumn(column));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_scroll_to", CallingConvention = CallingConvention.Cdecl)]
    public extern static void ScrollTo(this ColumnViewHandle columnView, uint pos, nint nil, ListScrollFlags flags, nint nil2);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle Type();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_append_column", CallingConvention = CallingConvention.Cdecl)]
    extern static void _AppendColumn(this ColumnViewHandle columnView, ColumnViewColumnHandle column);
}

