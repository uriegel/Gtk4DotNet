namespace GtkDotNet.SafeHandles;

public class StyleContextHandle : ObjectFloatingHandle
{
    public StyleContextHandle() : base() { }
    public StyleContextHandle(nint obj) : base() => SetInternalHandle(obj);
}
