namespace GtkDotNet.SafeHandles;

public class WebViewHandle : WidgetHandle
{
    public WebViewHandle() : base() { }
    public WebViewHandle(nint obj) : base() => SetInternalHandle(obj);
}
