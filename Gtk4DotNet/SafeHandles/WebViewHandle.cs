namespace GtkDotNet.SafeHandles;

public class WebViewHandle : WidgetHandle
{
    public WebViewHandle() : base() { }
    public WebViewHandle(nint obj) : base() => SetInternalHandle(obj);

    internal WebViewHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class WebViewHandleExtensions
{
    public static WebViewHandle DownCastWebView(this WidgetHandle widget) => new(widget);
}

