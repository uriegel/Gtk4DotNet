using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class ListView
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_view_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static ListViewHandle New(SingleSelectionHandle selectionModel, ListItemFactoryHandle listItemFactory);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_view_scroll_to", CallingConvention = CallingConvention.Cdecl)]
    public extern static void ScrollTo(this ListViewHandle listView, uint pos, ListScrollFlags flags, nint nil);
}

