using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class ColumnViewColumn
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_column_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static ColumnViewColumnHandle New(string title, ListItemFactoryHandle listItemFactory);
}

