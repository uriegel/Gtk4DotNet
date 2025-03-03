namespace GtkDotNet.SafeHandles;

public class ColumnViewColumnHandle : ObjectHandle
{
    public ColumnViewColumnHandle() : base() { }
    public ColumnViewColumnHandle(nint obj) : base() => SetInternalHandle(obj);
}
