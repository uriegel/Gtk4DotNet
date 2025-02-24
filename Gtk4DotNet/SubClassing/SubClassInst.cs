using CsTools.Extensions;
using GtkDotNet.SafeHandles;

namespace GtkDotNet.SubClassing;

public abstract class SubClassInst<THandle>
    where THandle : ObjectHandle
{
    public static implicit operator THandle(SubClassInst<THandle> obj) => obj.Handle;

    public static SubClassInst<THandle>? GetInstance(THandle handle)
        => objects.GetValue(handle.GetInternalHandle());
       
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

    protected abstract THandle CreateHandle(nint obj);

    static internal DisposeCallback finalizeDelegate = FinalizeHandler;

    static void FinalizeHandler(IntPtr obj)
    {
        objects[obj].OnFinalize();
        objects.Remove(obj);
    }

    readonly static Dictionary<IntPtr, SubClassInst<THandle>> objects = [];
}