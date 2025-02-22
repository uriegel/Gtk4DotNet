using GtkDotNet.SafeHandles;

namespace GtkDotNet.SubClassing;

public abstract class SubClassInst<THandle>
    where THandle : ObjectHandle
{
    public static implicit operator THandle(SubClassInst<THandle> obj) => obj.Handle;

    public THandle Handle { get; }

    protected SubClassInst(nint obj)
    {
        Handle = CreateHandle(obj);
        objects[obj] = this;
        OnCreate();
    }

    protected virtual void OnCreate() { }
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