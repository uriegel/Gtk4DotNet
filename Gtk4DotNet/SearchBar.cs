using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class SearchBar
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_search_bar_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static SearchBarHandle New();

    public static SearchBarHandle Child(this SearchBarHandle searchBar, WidgetHandle child)
        => searchBar.SideEffect(s => s.SetChild(child));

    public static SearchBarHandle SearchMode(this SearchBarHandle searchBar, bool set)
        => searchBar.SideEffect(s => s.SetSearchMode(set));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_search_bar_set_child", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetChild(this SearchBarHandle searchBar, WidgetHandle child);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_search_bar_set_search_mode", CallingConvention = CallingConvention.Cdecl)]
    extern static void SetSearchMode(this SearchBarHandle searchBar, bool set);
}

