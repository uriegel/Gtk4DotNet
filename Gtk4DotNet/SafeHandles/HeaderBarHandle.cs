namespace GtkDotNet.SafeHandles;

public class HeaderBarHandle : WidgetHandle
{
    public HeaderBarHandle() : base() { }
    public HeaderBarHandle(nint obj) : base() => SetInternalHandle(obj);

    internal HeaderBarHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class HeaderBarHandleExtensions
{
    public static HeaderBarHandle DownCastHeaderBar(this WidgetHandle widget) => new(widget);
}

