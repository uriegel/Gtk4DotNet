using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class ListItem : GObject
{
    public void SetChild(Widget widget) => SetChild(this, widget);

    public T GetChild<T>()
        where T : Widget, new()
    {
        var ptr = GetChild(this);
        var t = new T();
        t.SetInternalHandle(ptr);
        t.AutoDestroyed = true;
        return t;
    }

    public T? GetItem<T>()
        where T : class
    {
        var obj = GetItem(this);
        obj.AutoDestroyed = true;
        return obj.GetManagedData<T>(ListStore.DATA);
    }
        
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_item_set_child", CallingConvention = CallingConvention.Cdecl)]
    public extern static void SetChild(ListItem listItem, Widget widget);

    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_item_get_child", CallingConvention = CallingConvention.Cdecl)]
    public extern static nint GetChild(ListItem listItem);
    
    [DllImport(Libs.LibGtk, EntryPoint = "gtk_list_item_get_item", CallingConvention = CallingConvention.Cdecl)]
    extern static GObject GetItem(ListItem listItem);
}
