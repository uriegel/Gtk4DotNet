namespace GtkDotNet.SafeHandles;

public class PanedHandle : WidgetHandle
{
    public PanedHandle() : base() { }
    public PanedHandle(nint obj) : base() => SetInternalHandle(obj);

    internal PanedHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class PanedHandleExtensions
{
    public static PanedHandle DownCastPaned(this WidgetHandle widget) => new(widget);
}

