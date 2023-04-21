using System;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class SearchBar
{
    [DllImport(Globals.LibGtk, EntryPoint = "gtk_search_bar_set_search_mode", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetSearchMode(IntPtr searchBar, bool set);
}

