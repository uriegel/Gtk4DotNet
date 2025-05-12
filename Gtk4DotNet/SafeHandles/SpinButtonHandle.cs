namespace GtkDotNet.SafeHandles;

public class SpinButtonHandle : WidgetHandle
{
    public SpinButtonHandle() : base() { }
    public SpinButtonHandle(nint obj) : base() => SetInternalHandle(obj);
    internal SpinButtonHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class SpinButtonHandleExtensions
{
    public static SpinButtonHandle DownCastSpinButton(this WidgetHandle widget) => new(widget);
}


