using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using LinqTools;

namespace GtkDotNet;

public static class SearchBar
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_search_bar_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static SearchBarHandle New();

    public static SearchBarHandle Child(this SearchBarHandle searchBar, WidgetHandle child)
        => searchBar.SideEffect(s => s.SetChild(child));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_search_bar_set_child", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetChild(this SearchBarHandle searchBar, WidgetHandle child);
}

