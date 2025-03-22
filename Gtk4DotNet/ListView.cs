using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class ListView
{
    public static ListViewHandle New(IListModel selectionModel, ListItemFactoryHandle listItemFactory)
        => New(selectionModel.GetInternalHandle(), listItemFactory);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_view_scroll_to", CallingConvention = CallingConvention.Cdecl)]
    public extern static void ScrollTo(this ListViewHandle listView, int pos, ListScrollFlags flags, nint nil);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_view_get_type", CallingConvention = CallingConvention.Cdecl)]
    public static extern GTypeHandle Type();

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_view_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ListViewHandle New(nint selectionModel, ListItemFactoryHandle listItemFactory);
}

