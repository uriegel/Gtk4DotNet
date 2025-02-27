using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class ListItem
{
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_item_set_child", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetChild(this ListItemHandle listItem, WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_item_get_child", CallingConvention = CallingConvention.Cdecl)]
    public extern static LabelHandle GetChild(this ListItemHandle listItem);
}

