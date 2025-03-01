namespace GtkDotNet.SafeHandles;

public class ApplicationWindowHandle : WindowHandle, IActionMap
{
    public ApplicationWindowHandle() : base() { }
    public ApplicationWindowHandle(nint obj) : base() => SetInternalHandle(obj);
}
