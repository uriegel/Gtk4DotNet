namespace GtkDotNet.SafeHandles;

public class ButtonHandle : WidgetHandle
{
    public ButtonHandle() : base() { }
    public ButtonHandle(nint obj) : base() => SetInternalHandle(obj);

    internal ButtonHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class ButtonHandleExtensions
{
    public static ButtonHandle DownCastButton(this WidgetHandle widget) => new(widget);
}

