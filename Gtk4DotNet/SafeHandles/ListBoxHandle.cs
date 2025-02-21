namespace GtkDotNet.SafeHandles;

public class ListBoxHandle : WidgetHandle
{
    public ListBoxHandle() : base() { }
    public ListBoxHandle(nint obj) : base() => SetInternalHandle(obj);
}

