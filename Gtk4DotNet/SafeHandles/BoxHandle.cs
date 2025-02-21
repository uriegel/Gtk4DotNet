namespace GtkDotNet.SafeHandles;

public class BoxHandle : WidgetHandle
{
    public BoxHandle() : base() { }
    public BoxHandle(nint obj) : base() => SetInternalHandle(obj);
}
