namespace GtkDotNet.SafeHandles;

public class WindowHandle : WidgetHandle
{
    public WindowHandle() : base() { }
    public WindowHandle(nint obj) : base() => SetInternalHandle(obj);

    internal WindowHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class WindowHandleExtensions
{
    public static WindowHandle DownCastWindow(this WidgetHandle widget) => new(widget);
}

