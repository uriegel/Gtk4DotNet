using System.Runtime.InteropServices;
using CsTools.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public interface IListModel
{
    public IListModel Append(ObjectHandle obj)
    {
        _Append(GetInternalHandle(), obj);
        GObject.Unref(obj.GetInternalHandle());
        obj.IsFloating = true;
        return this;
    }

    public IListModel Append<T>(T t)
    {
        var obj = GObject.New<GObjectHandle>(GObject.Type());
        obj.IsFloating = true;
        var gchandle = GCHandle.Alloc(t, GCHandleType.Normal);
        var ptr = GCHandle.ToIntPtr(gchandle);
        obj.SetData("managedObject", ptr);
        AddWeakRef(obj);
        _Append(GetInternalHandle(), obj);
        return this;
    }

    public IListModel Splice(ObjectHandle[] objs)
    {
        Splice(0, 0, objs);
        return this;
    }

    public IListModel Splice2(uint pos, ObjectHandle[] objs)
    {
        Splice(pos, 0, objs);
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
                    var obj = GObject.New<GObjectHandle>(GObject.Type());
                    obj.IsFloating = true;
                    var gchandle = GCHandle.Alloc(o, GCHandleType.Normal);
                    var ptr = GCHandle.ToIntPtr(gchandle);
                    obj.SetData("managedObject", ptr);
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

    public IListModel Splice(uint pos, uint removals, ObjectHandle[] objs)
    {
        var unmanagedPtr = MakeObjArray(objs);
        _Splice(GetInternalHandle(), pos, removals, unmanagedPtr, objs.Length);
        Marshal.FreeHGlobal(unmanagedPtr);
        foreach (var obj in objs)
        {
            GObject.Unref(obj.GetInternalHandle());
            obj.IsFloating = true;
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

    void InternalSplice(uint pos, uint removals, ObjectHandle[] objs)
    {
        var unmanagedPtr = MakeObjArray(objs, objs.Length);
        _Splice(GetInternalHandle(), pos, removals, unmanagedPtr, objs.Length);
        Marshal.FreeHGlobal(unmanagedPtr);
        foreach (var obj in objs)
        {
            GObject.Unref(obj.GetInternalHandle());
            obj.IsFloating = true;
        }
    }

    static nint MakeObjArray(IEnumerable<ObjectHandle> objs, int count)
    {
        var unmanagedPtr = Marshal.AllocHGlobal(nint.Size * count);

        var i = 0;
        foreach (var obj in objs)
            Marshal.WriteIntPtr(unmanagedPtr, i++ * IntPtr.Size, obj.GetInternalHandle());

        return unmanagedPtr;
    }

    static nint MakeObjArray(ObjectHandle[] objs)
    {
        var unmanagedPtr = Marshal.AllocHGlobal(nint.Size * objs.Length);
        for (int i = 0; i < objs.Length; i++)
            Marshal.WriteIntPtr(unmanagedPtr, i * IntPtr.Size, objs[i].GetInternalHandle());

        return unmanagedPtr;
    }

    private static readonly TwoPointerDelegate OnDisposeDelegate = OnDispose;

    static void AddWeakRef(ObjectHandle obj)
        => obj.AddWeakRef(Marshal.GetFunctionPointerForDelegate(OnDisposeDelegate), IntPtr.Zero);

    static void OnDispose(nint n, nint n2)
    {
        var ptr = GetData(n2, "managedObject");
        var gcHandle = GCHandle.FromIntPtr(ptr);
        gcHandle.Free();
    }

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_store_splice", CallingConvention = CallingConvention.Cdecl)]
    extern static void _Splice(nint model, uint pos, uint removalCount, nint nullArray, int length);

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_store_append", CallingConvention = CallingConvention.Cdecl)]
    extern static void _Append(nint model, ObjectHandle obj);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_weak_ref", CallingConvention = CallingConvention.Cdecl)]
    extern static void AddWeakRef(nint obj, nint finalizer, nint zero);

    [DllImport(Libs.LibGtk, EntryPoint = "g_object_get_data", CallingConvention = CallingConvention.Cdecl)]
    extern static nint GetData(nint obj, string key);
   
}
