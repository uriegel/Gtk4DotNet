namespace GtkDotNet.SafeHandles;

public class CustomColumnViewHandle : ScrolledWindowHandle
{
    public CustomColumnViewHandle() : base() { }
    public CustomColumnViewHandle(nint obj) : base() => SetInternalHandle(obj);

    internal CustomColumnViewHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class CustomColumnViewHandleExtensions
{
    public static CustomColumnViewHandle DownCastCustomColumnView(this WidgetHandle widget) => new(widget);
}

