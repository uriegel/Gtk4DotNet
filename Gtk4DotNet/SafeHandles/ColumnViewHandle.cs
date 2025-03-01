namespace GtkDotNet.SafeHandles;

public class ColumnViewHandle : ListBaseHandle
{
    public ColumnViewHandle() : base() { }
    public ColumnViewHandle(nint obj) : base() => SetInternalHandle(obj);
}

