namespace GtkDotNet.SafeHandles;

public class ColumnViewHandle : ListBaseHandle
{
    public ColumnViewHandle() : base() { }
    public ColumnViewHandle(nint obj) : base() => SetInternalHandle(obj);

    internal ColumnViewHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class ColumnViewHandleExtensions
{
    public static ColumnViewHandle DownCastColumnView(this WidgetHandle widget) => new(widget);
}


