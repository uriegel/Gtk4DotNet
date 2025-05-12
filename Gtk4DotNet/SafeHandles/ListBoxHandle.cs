namespace GtkDotNet.SafeHandles;

public class ListBoxHandle : WidgetHandle
{
    public ListBoxHandle() : base() { }
    public ListBoxHandle(nint obj) : base() => SetInternalHandle(obj);

    internal ListBoxHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class ListBoxHandleExtensions
{
    public static ListBoxHandle DownCastListBox(this WidgetHandle widget) => new(widget);
}




