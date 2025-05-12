namespace GtkDotNet.SafeHandles;

public class ScrolledWindowHandle : WidgetHandle
{
    public ScrolledWindowHandle() : base() { }
    public ScrolledWindowHandle(nint obj) : base() => SetInternalHandle(obj);

    internal ScrolledWindowHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class ScrolledWindowHandleExtensions
{
    public static ScrolledWindowHandle DownCastScrolledWindow(this WidgetHandle widget) => new(widget);
}

