namespace GtkDotNet.SafeHandles;

public class AppInfoHandle : ObjectHandle
{
    public AppInfoHandle() : base() { }
    public AppInfoHandle(nint obj) : base() => SetInternalHandle(obj);
}