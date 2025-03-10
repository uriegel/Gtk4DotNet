using System.Runtime.InteropServices;
using CsTools.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class ColumnView
{
    public static ColumnViewHandle New(IListModel selectionModel)
        => New(selectionModel.GetInternalHandle());
    public static ColumnViewHandle New()
        => New(0);

    public static ColumnViewHandle SetModel(this ColumnViewHandle columnView, IListModel selectionModel)
        => columnView.SideEffect(c => c.SetModel(selectionModel.GetInternalHandle()));

    public static THandle GetModel<THandle>(this ColumnViewHandle columnView)
        where THandle : SelectionHandle, new()
    {
        var res = new THandle();
        res.SetInternalHandle(GetModel(columnView));
        return res;
    }

    public static ColumnViewHandle OnActivate(this ColumnViewHandle columnView, Action<uint> onActivate)
        => columnView.SideEffect(cv => Gtk.SignalConnect<ActivateDelegate>(cv, "activate", (_, pos, __) => onActivate(pos)));

    public static ColumnViewHandle AppendColumn(this ColumnViewHandle columnView, ColumnViewColumnHandle column)
        => columnView.SideEffect(c => c._AppendColumn(column));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_get_sorter", CallingConvention = CallingConvention.Cdecl)]
    public extern static CustomSorterHandle GetSorter(this ColumnViewHandle columnView);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_scroll_to", CallingConvention = CallingConvention.Cdecl)]
    public extern static void ScrollTo(this ColumnViewHandle columnView, uint pos, nint nil, ListScrollFlags flags, nint nil2);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle Type();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_remove_column", CallingConvention = CallingConvention.Cdecl)]
    public extern static void RemoveColumn(this ColumnViewHandle columnView, ColumnViewColumnHandle column);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ColumnViewHandle New(nint selectionModel);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_append_column", CallingConvention = CallingConvention.Cdecl)]
    extern static void _AppendColumn(this ColumnViewHandle columnView, ColumnViewColumnHandle column);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_set_model", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetModel(this ColumnViewHandle columnView, nint selectionModel);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_get_model", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetModel(this ColumnViewHandle columnView);
}

delegate void ActivateDelegate(IntPtr p, uint pos, IntPtr pp);