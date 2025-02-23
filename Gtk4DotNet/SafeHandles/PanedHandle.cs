namespace GtkDotNet.SafeHandles;

public class PanedHandle : WidgetHandle
{
    public PanedHandle() : base() { }
    public PanedHandle(nint obj) : base() => SetInternalHandle(obj);
}
