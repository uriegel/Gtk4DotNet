namespace GtkDotNet.SafeHandles;

public class ApplicationWindowHandle : WindowHandle, IActionMap
{
    public ApplicationWindowHandle() : base() { }
    public ApplicationWindowHandle(nint obj) : base() => SetInternalHandle(obj);

    internal ApplicationWindowHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class ApplicationWindowHandleExtensions
{
    public static ApplicationWindowHandle DownCastApplicationWindow(this WidgetHandle widget) => new(widget);
}

