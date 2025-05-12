namespace GtkDotNet.SafeHandles;

public class GridHandle : WidgetHandle
{
    public GridHandle() : base() { }
    public GridHandle(nint obj) : base() => SetInternalHandle(obj);

    internal GridHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class GridHandleExtensions
{
    public static GridHandle DownCastGrid(this WidgetHandle widget) => new(widget);
}

