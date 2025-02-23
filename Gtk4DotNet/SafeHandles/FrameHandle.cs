namespace GtkDotNet.SafeHandles;

public class FrameHandle : WidgetHandle
{
    public FrameHandle() : base() { }
    public FrameHandle(nint obj) : base() => SetInternalHandle(obj);
}
