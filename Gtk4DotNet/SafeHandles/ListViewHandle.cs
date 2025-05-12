namespace GtkDotNet.SafeHandles;

public class ListViewHandle : ListBaseHandle
{
    public ListViewHandle() : base() { }
    public ListViewHandle(nint obj) : base() => SetInternalHandle(obj);

    internal ListViewHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class ListViewHandleExtensions
{
    public static ListViewHandle DownCastListView(this WidgetHandle widget) => new(widget);
}


