namespace GtkDotNet.SafeHandles;

public class AdwApplicationWindowHandle : ApplicationWindowHandle, IActionMap
{
    public AdwApplicationWindowHandle() : base() { }
    public AdwApplicationWindowHandle(nint obj) : base() => SetInternalHandle(obj);

    internal AdwApplicationWindowHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class AdwApplicationWindowHandleExtensions
{
    public static AdwApplicationWindowHandle DownCastAdwApplicationWindow(this WidgetHandle widget) => new(widget);
}


