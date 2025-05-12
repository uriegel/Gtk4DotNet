namespace GtkDotNet.SafeHandles;

public class ListBaseHandle : WidgetHandle
{
    public ListBaseHandle() : base() { }
    public ListBaseHandle(nint obj) : base() => SetInternalHandle(obj);

    internal ListBaseHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class ListBaseHandleExtensions
{
    public static ListBaseHandle DownCastListBase(this WidgetHandle widget) => new(widget);
}


