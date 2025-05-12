namespace GtkDotNet.SafeHandles;

public class ToggleButtonHandle : ButtonHandle
{
    public ToggleButtonHandle() : base() { }
    public ToggleButtonHandle(nint obj) : base() => SetInternalHandle(obj);

    internal ToggleButtonHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class ToggleButtonHandleExtensions
{
    public static ToggleButtonHandle DownCastToggleButton(this WidgetHandle widget) => new(widget);
}

