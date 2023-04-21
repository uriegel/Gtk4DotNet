using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace GtkDotNet;

public class ListStore<T>
{
    internal IntPtr Store { get; }

    public ListStore() => Store = RawListStore.New(GManaged.Type);

    public T GetListItem(IntPtr listItem)
        => GManaged<T>.GetValue(ListItem.GetItem(listItem));

    public T GetObject(int position)
    {
        var intptr = RawListStore.GetObject(Store, position);
        var result =  GManaged<T>.GetValue(intptr);
        GObject.Unref(intptr);
        return result;
    }

    public void Splice(IEnumerable<T> additions)
    {
        var items = additions.Select(n => GManaged<T>.New(n)).ToArray();
        RawListStore.Splice(Store, 0, 0, items, items.Length);
        for (var i = 0; i < items.Length; i++)
            GObject.Unref(items[i]);    
    }

    public void RemoveAll() => RawListStore.RemoveAll(Store);
}

static class RawListStore
{
    [DllImport(Globals.LibGtk, EntryPoint="g_list_store_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr New(long type);

    [DllImport(Globals.LibGtk, EntryPoint="g_list_model_get_object", CallingConvention = CallingConvention.Cdecl)]
    public extern static IntPtr GetObject(IntPtr store, int position);

    [DllImport(Globals.LibGtk, EntryPoint="g_list_store_remove_all", CallingConvention = CallingConvention.Cdecl)]
    public extern static void RemoveAll(IntPtr store);

    [DllImport(Globals.LibGtk, EntryPoint="g_list_store_splice", CallingConvention = CallingConvention.Cdecl)]
    public extern static void Splice(IntPtr store, int position, int countRemovals, 
        [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)][In] IntPtr[] additions, int countAdditions);
}

