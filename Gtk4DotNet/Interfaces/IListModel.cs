using System.Runtime.InteropServices;
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

    public IListModel Splice(ObjectHandle[] objs)
    {
        Splice(0, 0, objs);
        return this;
    }

    public IListModel Splice(uint pos, ObjectHandle[] objs)
    {
        Splice(pos, 0, objs);
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

    static nint MakeObjArray(ObjectHandle[] objs)
    {
        var unmanagedPtr = Marshal.AllocHGlobal(nint.Size * objs.Length);
        for (int i = 0; i < objs.Length; i++)
            Marshal.WriteIntPtr(unmanagedPtr, i * IntPtr.Size, objs[i].GetInternalHandle());

        return unmanagedPtr;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_store_splice", CallingConvention = CallingConvention.Cdecl)]
    extern static void _Splice(nint model, uint pos, uint removalCount, nint nullArray, int length);

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_store_append", CallingConvention = CallingConvention.Cdecl)]
    extern static void _Append(nint model, ObjectHandle obj);
    
    [DllImport(Libs.LibGtk, EntryPoint="g_object_weak_ref", CallingConvention = CallingConvention.Cdecl)]
    extern static void AddWeakRef(nint obj, nint finalizer, nint zero);
}
