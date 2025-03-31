namespace GtkDotNet.SafeHandles;

public class AdwApplicationWindowHandle : ApplicationWindowHandle, IActionMap
{
    public AdwApplicationWindowHandle() : base() { }
    public AdwApplicationWindowHandle(nint obj) : base() => SetInternalHandle(obj);
}
