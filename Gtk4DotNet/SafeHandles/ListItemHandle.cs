namespace GtkDotNet.SafeHandles;

public class ListItemHandle : ObjectFloatingHandle
{
    public ListItemHandle() : base() { }
    public ListItemHandle(nint obj) : base() => SetInternalHandle(obj);
}

