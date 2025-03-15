using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;
using GtkDotNet.SubClassing;

namespace GtkDotNet;

public static class ListItem
{
    public static THandle GetItem<THandle>(this ListItemHandle listItem)
        where THandle : ObjectHandle, new()
    {
        var res = new THandle();
        res.IsFloating = true;
        res.SetInternalHandle(_GetItem(listItem));
        return res;
    }
    
    public static T? GetObject<T>(this ListItemHandle listItem)
        where T: class
    {
        var item = listItem.GetItem<GObjectHandle>();
        var ptr = item.GetData("managedObject");
        var gcHandle = GCHandle.FromIntPtr(ptr);
        return gcHandle.Target as T;
    }

    public static THandle GetChild<THandle>(this ListItemHandle listItem)
        where THandle : WidgetHandle, new()
    {
        var res = new THandle();
        res.SetInternalHandle(_GetChild(listItem));
        return res;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_item_set_child", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetChild(this ListItemHandle listItem, WidgetHandle widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_item_get_child", CallingConvention = CallingConvention.Cdecl)]
    public extern static nint _GetChild(this ListItemHandle listItem);
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_item_get_item", CallingConvention = CallingConvention.Cdecl)]
    extern static nint _GetItem(this ListItemHandle listItem);
}

