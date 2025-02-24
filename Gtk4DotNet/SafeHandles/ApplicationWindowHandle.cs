namespace GtkDotNet.SafeHandles;

public class ApplicationWindowHandle : WindowHandle
{
    public ApplicationWindowHandle() : base() { }
    public ApplicationWindowHandle(nint obj) : base() => SetInternalHandle(obj);
}
