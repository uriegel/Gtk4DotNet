namespace GtkDotNet.SafeHandles;

public class LabelHandle : WidgetHandle
{
    public LabelHandle() : base() { }
    public LabelHandle(nint obj) : base() => SetInternalHandle(obj);

    internal LabelHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class LabelHandleExtensions
{
    public static LabelHandle DownCastLabel(this WidgetHandle widget) => new(widget);
}

