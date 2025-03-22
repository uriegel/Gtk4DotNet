using System.Runtime.InteropServices;
using GtkDotNet.SafeHandles;

namespace GtkDotNet.SubClassing;

public abstract class SubClassInst<THandle>
    where THandle : ObjectHandle, new()
{
    public static implicit operator THandle(SubClassInst<THandle> obj) => obj.Handle;

    public static THandle Create(string name)
        => GObject.New<THandle>(name.TypeFromName());

    public THandle Handle { get; }

    protected SubClassInst(nint obj) => Handle = CreateHandle(obj);
    protected internal virtual void OnCreate() { }
    protected virtual void OnFinalize() { }
    protected virtual void OnSetProperty(int propId, nint value) { }
    protected virtual void OnGetProperty(int propId, nint value) { }

    protected abstract THandle CreateHandle(nint obj);

    static internal DisposeDelegate finalizeDelegate = FinalizeHandler;
    static internal SetPropertyDelegate setPropertyDelegate = SetProperty;
    static internal GetPropertyDelegate getPropertyDelegate = GetProperty;

    static void FinalizeHandler(IntPtr obj)
    {
        var retrievedHandle = GetInstanceGCHandle(obj);
        var inst = retrievedHandle.Target as SubClassInst<THandle>;
        inst?.OnFinalize();
        retrievedHandle.Free();
    }
    internal protected static SubClassInst<THandle>? GetInstance(IntPtr handle)
        => GetInstanceGCHandle(handle).Target as SubClassInst<THandle>;

    static void SetProperty(nint obj, int propId, nint value, nint pspec)
        => GetInstance(obj)?.OnSetProperty(propId, value);

    static void GetProperty(nint obj, int propId, nint value, nint pspec)
        => GetInstance(obj)?.OnGetProperty(propId, value);

    static GCHandle GetInstanceGCHandle(IntPtr handle)
    {
        var of = SubClass<THandle>.MemoryOffset;
        var ptr = Marshal.ReadIntPtr(handle, of);
        return GCHandle.FromIntPtr(ptr);
    }
}

public static class THandleExtensions
{
    public static SubClassInst<THandle>? GetInstance<THandle>(this THandle handle)
        where THandle : ObjectHandle, new()
        => SubClassInst<THandle>.GetInstance(handle.GetInternalHandle());
    
}

