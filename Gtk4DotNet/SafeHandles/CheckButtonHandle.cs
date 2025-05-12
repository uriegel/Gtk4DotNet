namespace GtkDotNet.SafeHandles;

public class CheckButtonHandle : WidgetHandle
{
    public CheckButtonHandle() : base() { }
    public CheckButtonHandle(nint obj) : base() => SetInternalHandle(obj);
    internal CheckButtonHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class CheckButtonHandleExtensions
{
    public static CheckButtonHandle DownCastCheckButton(this WidgetHandle widget) => new(widget);
}


