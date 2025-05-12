namespace GtkDotNet.SafeHandles;

public class FrameHandle : WidgetHandle
{
    public FrameHandle() : base() { }
    public FrameHandle(nint obj) : base() => SetInternalHandle(obj);

    internal FrameHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class FrameHandleExtensions
{
    public static FrameHandle DownCastFrame(this WidgetHandle widget) => new(widget);
}

