namespace GtkDotNet.SafeHandles;

public class BannerHandle : WidgetHandle
{
    public BannerHandle() : base() { }
    public BannerHandle(nint obj) : base() => SetInternalHandle(obj);
}
