namespace GtkDotNet.SafeHandles;

public class BoxHandle : WidgetHandle
{
    public BoxHandle() : base() { }
    public BoxHandle(nint obj) : base() => SetInternalHandle(obj);

    internal BoxHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class BoxHandleExtensions
{
    public static BoxHandle DownCastBox(this WidgetHandle widget) => new(widget);
}
