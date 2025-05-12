namespace GtkDotNet.SafeHandles;

public class SearchBarHandle : WidgetHandle
{
    public SearchBarHandle() : base() { }
    public SearchBarHandle(nint obj) : base() => SetInternalHandle(obj);

    internal SearchBarHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class SearchBarHandleExtensions
{
    public static SearchBarHandle DownCastSearchBar(this WidgetHandle widget) => new(widget);
}



