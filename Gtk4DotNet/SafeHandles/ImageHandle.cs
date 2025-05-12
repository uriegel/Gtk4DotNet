namespace GtkDotNet.SafeHandles;

public class ImageHandle : WidgetHandle
{
    public ImageHandle() : base() { }
    public ImageHandle(nint obj) : base() => SetInternalHandle(obj);

    internal ImageHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class ImageHandleExtensions
{
    public static ImageHandle DownCastImage(this WidgetHandle widget) => new(widget);
}


