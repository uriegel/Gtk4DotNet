using GtkDotNet.SafeHandles;

namespace GtkDotNet.SubClassing;

public abstract class SubClassInst<THandle>
    where THandle : ObjectHandle, IDisposable
{
    public THandle Handle { get; }

    protected SubClassInst(nint obj)
    {
        Handle = CreateHandle(obj);
    }

    public void Dispose() => Handle.Dispose();

    protected abstract THandle CreateHandle(nint obj);
}