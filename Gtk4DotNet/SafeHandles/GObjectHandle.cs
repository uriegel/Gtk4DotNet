namespace GtkDotNet.SafeHandles;

public class GObjectHandle : ObjectHandle
{
    public GObjectHandle(nint obj) : base() => SetInternalHandle(obj);
}
