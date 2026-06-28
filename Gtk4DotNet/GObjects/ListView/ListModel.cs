using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public abstract class ListModel : GObject
{
    public event OnItemsChangedDelegate OnItemsChanged
    {
        add
        {
            OnItemsChangedRawDelegate unmanagedDelegate = (_, position, removed, added, _) => value(position, removed, added);
            var id = SignalConnectForEvent("items-changed", unmanagedDelegate);
            eventDatas.TryAdd(value, new(id, value, unmanagedDelegate));
        }
        remove
        {
            if (eventDatas.Remove(value, out var data))
                SignalDisconnectEvent(data.Id);
        }
    }

    public IEnumerable<T> GetItems<T>()
        where T : class
    {
        int i = 0;
        while (true)
        {
            var item = GetItem<T>(i++);
            if (item == null)
                yield break;
            yield return item;
        }
    }

    public T? GetItem<T>(int position) where T : class
    {
        using var obj = GetItem(this, position);
        if (obj.IsInvalid)
            return null;
        return obj.GetManagedData<T>(ListStore.DATA);
    }

    public int GetItems() => GetItems(this);

    public int ItemsCount() => GetRawItems().Count();

    internal nint GetRawItem(int position)
    {
        using var obj = GetItem(this, position);
        if (obj.IsInvalid)
            return 0;
        return obj.GetManagedRawData(ListStore.DATA);
    }

    internal IEnumerable<nint> GetRawItems()
    {
        int i = 0;
        while (true)
        {
            var item = GetRawItem(i++);
            if (item == 0)
                yield break;
            yield return item;
        }
    }

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_model_get_item", CallingConvention = CallingConvention.Cdecl)]
    extern static GObject GetItem(ListModel model, int position);

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_model_get_item", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetRawItem(ListModel model, int position);

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_model_get_n_items", CallingConvention = CallingConvention.Cdecl)]
    extern static int GetItems(ListModel model);
}

public delegate void OnItemsChangedDelegate(int position, int removed, int added);

delegate void OnItemsChangedRawDelegate(nint _, int position, int removed, int added, nint __);