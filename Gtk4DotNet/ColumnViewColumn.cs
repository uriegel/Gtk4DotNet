using System.Runtime.InteropServices;
using CsTools.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class ColumnViewColumn
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_column_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static ColumnViewColumnHandle New(string title, ListItemFactoryHandle listItemFactory);

    public static ColumnViewColumnHandle SetSorter(this ColumnViewColumnHandle column, CustomSorterHandle sorter)
        => column.SideEffect(c => c._SetSorter(sorter));

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_column_view_column_set_sorter", CallingConvention = CallingConvention.Cdecl)]
    extern static ColumnViewColumnHandle _SetSorter(this ColumnViewColumnHandle column, CustomSorterHandle sorter);
}

