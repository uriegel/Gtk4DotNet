namespace GtkDotNet.SafeHandles;

public class ImageHandle : WidgetHandle
{
    public ImageHandle() : base() { }
    public ImageHandle(nint obj) : base() => SetInternalHandle(obj);
}
