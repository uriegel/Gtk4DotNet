using System.Runtime.InteropServices;
using CsTools.Extensions;

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
        SetManagedData(obj, Quark.ListData, t);
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

    public void Splice<T>(int pos, int removals, IEnumerable<T> objs)
    {
        var idx = 0;
        foreach (var obj in
            objs.Select(o =>
            {
                var obj = NewObject(Type(), 0);
                SetManagedData(obj, Quark.ListData, o);
                return obj;
            }).Windowed(9_000).Select(n => n.ToArray()))
        {
            InternalSplice(pos + idx, idx == 0 ? removals : 0, obj);
            idx += obj.Length;
        }
    }

    public void Remove(int position) => Remove(this, position);

    public void RemoveItems(int pos, int removals) => Splice(this, pos, removals, 0, 0);

    void InternalSplice(int pos, int removals, nint[] objs)
    {
        var unmanagedPtr = MakeObjArray(objs, objs.Length);
        Splice(this, pos, removals, unmanagedPtr, objs.Length);
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

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_store_splice", CallingConvention = CallingConvention.Cdecl)]
    extern static void Splice(ListStore store, int pos, int removalCount, nint nullArray, int length);
}