namespace GtkDotNet.SafeHandles;

public class OverlayHandle : WidgetHandle
{
    public OverlayHandle() : base() { }
    public OverlayHandle(nint obj) : base() => SetInternalHandle(obj);

    internal OverlayHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class OverlayHandleExtensions
{
    public static OverlayHandle DownCastPopover(this WidgetHandle widget) => new(widget);
}

