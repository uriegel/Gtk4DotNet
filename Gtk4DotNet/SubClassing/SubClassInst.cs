using CsTools.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet.SubClassing;

public abstract class SubClassInst<THandle>
    where THandle : ObjectHandle
{
    public static implicit operator THandle(SubClassInst<THandle> obj) => obj.Handle;

    public static SubClassInst<THandle>? GetInstance(THandle handle)
        => objects.GetValue(handle.GetInternalHandle());
    public static SubClassInst<THandle>? GetInstance(nint handle)
        => objects.GetValue(handle);
       
    public THandle Handle { get; }

    protected SubClassInst(nint obj)
    {
        Handle = CreateHandle(obj);
        objects[obj] = this;
        OnCreate();
    }

    protected virtual void OnCreate() { }
    internal protected virtual void Initialize() { }
    protected virtual void OnFinalize() { }
    protected virtual void OnSetProperty(uint propId, nint value) { }
    protected virtual void OnGetProperty(uint propId, nint value) { }

    protected abstract THandle CreateHandle(nint obj);

    static internal DisposeDelegate finalizeDelegate = FinalizeHandler;
    static internal SetPropertyDelegate setPropertyDelegate = SetProperty;
    static internal GetPropertyDelegate getPropertyDelegate = GetProperty;

    static void FinalizeHandler(IntPtr obj)
    {
        objects[obj].OnFinalize();
        objects.Remove(obj);
    }

    static void SetProperty(nint obj, uint propId, nint value, nint pspec)
        => objects[obj]?.OnSetProperty(propId, value);

    static void GetProperty(nint obj, uint propId, nint value, nint pspec)
        => objects[obj]?.OnGetProperty(propId, value);

    readonly static Dictionary<IntPtr, SubClassInst<THandle>> objects = [];
}