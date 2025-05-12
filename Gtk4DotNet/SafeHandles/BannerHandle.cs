namespace GtkDotNet.SafeHandles;

public class BannerHandle : WidgetHandle
{
    public BannerHandle() : base() { }
    public BannerHandle(nint obj) : base() => SetInternalHandle(obj);

    internal BannerHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class BannerHandleExtensions
{
    public static BannerHandle DownCastBanner(this WidgetHandle widget) => new(widget);
}

