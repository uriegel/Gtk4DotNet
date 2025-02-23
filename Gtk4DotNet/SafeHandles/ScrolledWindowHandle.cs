namespace GtkDotNet.SafeHandles;

public class ScrolledWindowHandle : WidgetHandle
{
    public ScrolledWindowHandle() : base() { }
    public ScrolledWindowHandle(nint obj) : base() => SetInternalHandle(obj);
}
