namespace GtkDotNet.SafeHandles;

public class ListBaseHandle : WidgetHandle
{
    public ListBaseHandle() : base() { }
    public ListBaseHandle(nint obj) : base() => SetInternalHandle(obj);
}

