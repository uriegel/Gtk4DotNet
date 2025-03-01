namespace GtkDotNet.SafeHandles;

public class ColumnViewColumnHandle : ObjectFloatingHandle
{
    public ColumnViewColumnHandle() : base() { }
    public ColumnViewColumnHandle(nint obj) : base() => SetInternalHandle(obj);
}
