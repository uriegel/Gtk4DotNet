namespace GtkDotNet.SafeHandles;

public class WidgetHandle : ObjectFloatingHandle
{
    public WidgetHandle() : base() { }
    public WidgetHandle(nint obj) : base() => SetInternalHandle(obj);
}
