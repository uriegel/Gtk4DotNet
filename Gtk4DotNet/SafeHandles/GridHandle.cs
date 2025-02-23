namespace GtkDotNet.SafeHandles;

public class GridHandle : WidgetHandle
{
    public GridHandle() : base() { }
    public GridHandle(nint obj) : base() => SetInternalHandle(obj);
}
