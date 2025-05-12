namespace GtkDotNet.SafeHandles;

public class DrawingAreaHandle : WidgetHandle
{
    public DrawingAreaHandle() : base() { }
    public DrawingAreaHandle(nint obj) : base() => SetInternalHandle(obj);

    internal DrawingAreaHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class DrawingAreaHandleExtensions
{
    public static DrawingAreaHandle DownCastDrawingArea(this WidgetHandle widget) => new(widget);
}


