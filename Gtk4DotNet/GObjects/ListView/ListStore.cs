using System.Runtime.InteropServices;
using CsTools.Extensions;

namespace Gtk4DotNet;

public class ListStore<T> : ListModel
{
    public ListStore()
    {
        var handle = ListStorePinvoke.New(Type());
        SetInternalHandle(handle);
        CheckDiagnostics();
        AutoDestroyed = true;
    }

    public ListStore<T> Append(T t)
    {
        var obj = ListStorePinvoke.NewObject(Type(), 0);
        SetManagedData(obj, Quark.ListData, t);
        ListStorePinvoke.Append(GetInternalHandle(), obj);
        Unref(obj);
        return this;
    }

    public void Initialize(IEnumerable<T> items)
    {
        foreach (var item in items)
            Append(item);
    }

    public void Splice(int pos, int removals, IEnumerable<T> objs)
    {
        var idx = 0;
        foreach (var obj in
            objs.Select(o =>
            {
                var obj = ListStorePinvoke.NewObject(Type(), 0);
                SetManagedData(obj, Quark.ListData, o);
                return obj;
            }).Windowed(9_000).Select(n => n.ToArray()))
        {
            InternalSplice(pos + idx, idx == 0 ? removals : 0, obj);
            idx += obj.Length;
        }
    }

    public void Remove(int position) => ListStorePinvoke.Remove(GetInternalHandle(), position);

    public void RemoveItems(int pos, int removals) => ListStorePinvoke.Splice(GetInternalHandle(), pos, removals, 0, 0);

    void InternalSplice(int pos, int removals, nint[] objs)
    {
        var unmanagedPtr = MakeObjArray(objs, objs.Length);
        ListStorePinvoke.Splice(GetInternalHandle(), pos, removals, unmanagedPtr, objs.Length);
        Marshal.FreeHGlobal(unmanagedPtr);
        foreach (var obj in objs)
            Unref(obj);

        static nint MakeObjArray(IEnumerable<nint> objs, int count)
        {
            var unmanagedPtr = Marshal.AllocHGlobal(nint.Size * count);

            var i = 0;
            foreach (var obj in objs)
                Marshal.WriteIntPtr(unmanagedPtr, i++ * IntPtr.Size, obj);

            return unmanagedPtr;
        }
    }

    public void RemoveAll() => ListStorePinvoke.RemoveAll(GetInternalHandle());
}

static class ListStorePinvoke
{
    [DllImport(Libs.LibGtk, EntryPoint = "g_list_store_new", CallingConvention = CallingConvention.Cdecl)]
    internal extern static nint New(nint type);

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_store_append", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void Append(nint store, nint obj);

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_store_remove", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void Remove(nint store, int position);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_new", CallingConvention = CallingConvention.Cdecl)]
    internal static extern nint NewObject(nint type, nint _);

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_store_remove_all", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void RemoveAll(nint store);

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_store_splice", CallingConvention = CallingConvention.Cdecl)]
    internal extern static void Splice(nint store, int pos, int removalCount, nint nullArray, int length);
}

