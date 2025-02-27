namespace GtkDotNet.SafeHandles;

public class StringObjectHandle : ObjectFloatingHandle
{
    public StringObjectHandle() : base() { }
    public StringObjectHandle(nint obj) : base() => SetInternalHandle(obj);
}
