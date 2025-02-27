namespace GtkDotNet.SafeHandles;

public class ListViewHandle : ListBaseHandle
{
    public ListViewHandle() : base() { }
    public ListViewHandle(nint obj) : base() => SetInternalHandle(obj);
}

