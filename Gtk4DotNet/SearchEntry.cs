using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class SearchEntry
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_search_entry_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static SearchEntryHandle New();
}


 