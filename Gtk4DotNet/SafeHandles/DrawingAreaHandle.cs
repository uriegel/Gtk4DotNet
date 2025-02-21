namespace GtkDotNet.SafeHandles;

public class DrawingAreaHandle : WidgetHandle
{
    public DrawingAreaHandle() : base() { }
    public DrawingAreaHandle(nint obj) : base() => SetInternalHandle(obj);
}
