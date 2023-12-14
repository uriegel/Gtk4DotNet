using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using CsTools.Extensions;

namespace GtkDotNet;

public static class SearchEntry 
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_search_entry_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static SearchEntryHandle New();

    public static SearchEntryHandle OnSearchChanged(this SearchEntryHandle entry, Action<SearchEntryHandle> onSearchChanged)
    => entry.SideEffect(e => Gtk.SignalConnect<TwoPointerDelegate>(e, "search-changed", (IntPtr _, IntPtr __)  => onSearchChanged(e)));
}

 