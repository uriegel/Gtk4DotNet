using System.Runtime.InteropServices;

namespace Gtk4DotNet;

public class ListStore : ListModel
{
    public static ListStore New()
    {
        var res = New(Type());
        res.CheckDiagnostics();
        res.AutoDestroyed = true;
        return res;
    }

    public ListStore Append<T>(T t)
        where T : class
    {
        var obj = NewObject(Type(), 0);
        SetManagedData(obj, DATA, t);
        Append(this, obj);
        Unref(obj);
        return this;
    }

    public void Initialize<T>(IEnumerable<T> items)
        where T : class
    {
        foreach (var item in items)
            Append(item);
    }

    public void Remove(int position) => Remove(this, position);

    public void RemoveAll() => RemoveAll(this);

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_store_new", CallingConvention = CallingConvention.Cdecl)]
    extern static ListStore New(nint type);

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_store_append", CallingConvention = CallingConvention.Cdecl)]
    extern static void Append(ListStore store, nint obj);

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_store_remove", CallingConvention = CallingConvention.Cdecl)]
    extern static void Remove(ListStore store, int position);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_new", CallingConvention = CallingConvention.Cdecl)]
    static extern nint NewObject(nint type, nint _);

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_store_remove_all", CallingConvention = CallingConvention.Cdecl)]
    extern static void RemoveAll(ListStore store);

    internal const string DATA = "LIST_STORE_DATA";
}
        