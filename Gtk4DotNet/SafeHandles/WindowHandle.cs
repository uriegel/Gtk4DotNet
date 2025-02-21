namespace GtkDotNet.SafeHandles;

public class WindowHandle : WidgetHandle
{
    public WindowHandle() : base() { }
    public WindowHandle(nint obj) : base() => SetInternalHandle(obj);
}
