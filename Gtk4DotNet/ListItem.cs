using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class ListItem
{
    public static string? GetStringItem(this ListItemHandle listItem)
    {
        var ptr = GetItem(listItem);
        return ptr.GetString();
    }

    public static T? GetObject<T>(this ListItemHandle listItem)
        where T : class
    {
        var item = listItem.GetItem();
        var ptr = item.GetData(MANAGED_OBJECT);
        var gcHandle = GCHandle.FromIntPtr(ptr);
        return gcHandle.Target as T;
    }

    public static nint GetRawItem(this ListItemHandle listItem)
        => GetItem(listItem);

    public static THandle GetChild<THandle>(this ListItemHandle listItem)
        where THandle : WidgetHandle, new()
    {
        var res = new THandle();
        res.SetInternalHandle(_GetChild(listItem));
        return res;
    }

    internal const string MANAGED_OBJECT = "ManagedObject";

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_item_set_child", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetChild(this ListItemHandle listItem, WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_item_get_child", CallingConvention = CallingConvention.Cdecl)]
    public extern static nint _GetChild(this ListItemHandle listItem);
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_item_get_item", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetItem(this ListItemHandle listItem);
}

