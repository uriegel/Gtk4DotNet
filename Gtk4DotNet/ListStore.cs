using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet;

public static class ListStore
{
    [DllImport(Libs.LibGtk, EntryPoint = "g_list_store_new", CallingConvention = CallingConvention.Cdecl)]
    public extern static ListModelHandle New(GTypeHandle type);

    public static ListModelHandle Append(this ListModelHandle model, ObjectHandle obj)
    {
        model._Append(obj);
        GObject.Unref(obj.GetInternalHandle());
        obj.IsFloating = true;
        return model;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_store_remove_all", CallingConvention = CallingConvention.Cdecl)]
    public extern static void RemoveAll(this ListModelHandle model);

    public static ListModelHandle Splice(this ListModelHandle model, ObjectHandle[] objs)
        => Splice(model, 0, 0, objs);

    public static ListModelHandle Splice(this ListModelHandle model, uint pos, ObjectHandle[] objs)
        => Splice(model, pos, 0, objs);

    public static ListModelHandle Splice(this ListModelHandle model, uint pos, uint removals, ObjectHandle[] objs)
    {
        var unmanagedPtr = MakeObjArray(objs);
        model._Splice(pos, removals, unmanagedPtr, objs.Length);
        Marshal.FreeHGlobal(unmanagedPtr);
        foreach (var obj in objs)
        {
            GObject.Unref(obj.GetInternalHandle());
            obj.IsFloating = true;
        }
        return model;
    }

    public static ListModelHandle RemoveItems(this ListModelHandle model, uint pos, uint removals)
    {
        model._Splice(pos, removals, 0, 0);
        return model;
    }

    static nint MakeObjArray(ObjectHandle[] objs)
    {
        var unmanagedPtr = Marshal.AllocHGlobal(nint.Size * objs.Length);
        for (int i = 0; i < objs.Length; i++)
            Marshal.WriteIntPtr(unmanagedPtr, i * IntPtr.Size, objs[i].GetInternalHandle());

        return unmanagedPtr;
    }

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_store_splice", CallingConvention = CallingConvention.Cdecl)]
    extern static void _Splice(this ListModelHandle model, uint pos, uint removalCount, nint nullArray, int length);

    [DllImport(Libs.LibGtk, EntryPoint = "g_list_store_append", CallingConvention = CallingConvention.Cdecl)]
    extern static void _Append(this ListModelHandle model, ObjectHandle obj);
}
        