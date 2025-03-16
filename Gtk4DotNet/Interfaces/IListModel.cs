using System.Runtime.InteropServices;
using CsTools.Extensions;

namespace GtkDotNet;

public interface IListModel
{
    public IListModel Append<T>(T t)
    {
        var obj = GObject.New(GObject.Type(), 0);
        var gchandle = GCHandle.Alloc(t, GCHandleType.Normal);
        var ptr = GCHandle.ToIntPtr(gchandle);
        obj.SetData(ListItem.MANAGED_OBJECT, ptr);
        AddWeakRef(obj);
        _Append(GetInternalHandle(), obj);
        return this;
    }

    public IListModel Splice<T>(IEnumerable<T> objs)
    {
        Splice(0, 0, objs);
        return this;
    }

    public IListModel Splice<T>(uint pos, IEnumerable<T> objs)
    {
        Splice(pos, 0, objs);
        return this;
    }

    public IListModel Splice<T>(uint pos, uint removals, IEnumerable<T> objs)
    {
        uint idx = 0;
        foreach (var obj in
            objs.Select(o =>
                {
                    var obj = GObject.New(GObject.Type(), 0);
                    var gchandle = GCHandle.Alloc(o, GCHandleType.Normal);
                    var ptr = GCHandle.ToIntPtr(gchandle);
                    obj.SetData(ListItem.MANAGED_OBJECT, ptr);
                    AddWeakRef(obj);
                    return obj;
                })
                    .Windowed(9_000).Select(n => n.ToArray()))
        {
            InternalSplice(pos + idx, idx == 0 ? removals : 0, obj);
            idx += (uint)obj.Length;
        }

        return this;
    }

    public IListModel RemoveItems(uint pos, uint removals)
    {
        _Splice(GetInternalHandle(), pos, removals, 0, 0);
        return this;
    }

    public nint GetInternalHandle();

    public bool IsFloating { get; set; }

    public IListModel AddWeakRef(Action onDisposing)
    {
        var key = GtkDelegates.GetKey();
        TwoPointerDelegate callback = (_, ___) =>
        {
            GtkDelegates.Remove(key);
            onDisposing();
        };
        GtkDelegates.Add(key, callback);
        AddWeakRef(GetInternalHandle(), Marshal.GetFunctionPointerForDelegate(callback as Delegate), IntPtr.Zero);
        return this;
    }

    public void Dispose();

    void InternalSplice(uint pos, uint removals, nint[] objs)
    {
        var unmanagedPtr = MakeObjArray(objs, objs.Length);
        _Splice(GetInternalHandle(), pos, removals, unmanagedPtr, objs.Length);
        Marshal.FreeHGlobal(unmanagedPtr);
        foreach (var obj in objs)
            GObject.Unref(obj);
    }

    static nint MakeObjArray(IEnumerable<nint> objs, int count)
    {
        var unmanagedPtr = Marshal.AllocHGlobal(nint.Size * count);

        var i = 0;
        foreach (var obj in objs)
            Marshal.WriteIntPtr(unmanagedPtr, i++ * IntPtr.Size, obj);

        return unmanagedPtr;
    }

    private static readonly TwoPointerDelegate OnDisposeDelegate = OnDispose;

    static void AddWeakRef(nint obj)
        => obj.AddWeakRef(Marshal.GetFunctionPointerForDelegate(OnDisposeDelegate), IntPtr.Zero);

    static void OnDispose(nint n, nint n2)
    {
        var ptr = GetData(n2, ListItem.MANAGED_OBJECT);
        var gcHandle = GCHandle.FromIntPtr(ptr);
        gcHandle.Free();
    }

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_store_splice", CallingConvention = CallingConvention.Cdecl)]
    extern static void _Splice(nint model, uint pos, uint removalCount, nint nullArray, int length);

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_store_append", CallingConvention = CallingConvention.Cdecl)]
    extern static void _Append(nint model, nint obj);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_weak_ref", CallingConvention = CallingConvention.Cdecl)]
    extern static void AddWeakRef(nint obj, nint finalizer, nint zero);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_get_data", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetData(nint obj, string key);
   
}
