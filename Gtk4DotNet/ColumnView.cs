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

    public static ColumnViewHandle TabBehavior(this ColumnViewHandle columnView, ListTabBehavior behavior)
        => columnView.SideEffect(cv => cv.SetTabBehavior(behavior));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_get_tab_behavior", CallingConvention = CallingConvention.Cdecl)]
    public extern static ListTabBehavior GetTabBehavior(this ColumnViewHandle columnView);

    public static ColumnViewHandle OnActivate(this ColumnViewHandle columnView, Action<int> onActivate)
        => columnView.SideEffect(cv => Gtk.SignalConnect<ActivateDelegate>(cv, "activate", (_, pos, __) => onActivate(pos)));

    public static void ScrollTo(this ColumnViewHandle columnView, int pos, ListScrollFlags flags)
        => ScrollTo(columnView, pos, 0, flags, 0);
    public static ColumnViewHandle AppendColumn(this ColumnViewHandle columnView, ColumnViewColumnHandle column)
        => columnView.SideEffect(c => c._AppendColumn(column));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_get_sorter", CallingConvention = CallingConvention.Cdecl)]
    public extern static CustomSorterHandle GetSorter(this ColumnViewHandle columnView);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle Type();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_remove_column", CallingConvention = CallingConvention.Cdecl)]
    public extern static void RemoveColumn(this ColumnViewHandle columnView, ColumnViewColumnHandle column);

    public static void EnableRubberband(this ColumnViewHandle columnView)
        => EnableRubberband(columnView, true);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_set_enable_rubberband", CallingConvention = CallingConvention.Cdecl)]
    public extern static void EnableRubberband(this ColumnViewHandle columnView, bool enable);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ColumnViewHandle New(nint selectionModel);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_append_column", CallingConvention = CallingConvention.Cdecl)]
    extern static void _AppendColumn(this ColumnViewHandle columnView, ColumnViewColumnHandle column);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_set_model", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void SetModel(this ColumnViewHandle columnView, nint selectionModel);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_get_model", CallingConvention = CallingConvention.Cdecl)]
    internal extern static nint GetModel(this ColumnViewHandle columnView);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_scroll_to", CallingConvention = CallingConvention.Cdecl)]
    extern static void ScrollTo(this ColumnViewHandle columnView, int pos, nint nilc, ListScrollFlags flags, nint nil);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_set_tab_behavior", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetTabBehavior(this ColumnViewHandle columnView, ListTabBehavior behavior);
}

delegate void ActivateDelegate(IntPtr p, int pos, IntPtr pp);