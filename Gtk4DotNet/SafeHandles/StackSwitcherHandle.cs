namespace GtkDotNet.SafeHandles;

public class StackSwitcherHandle : BoxHandle
{
    public StackSwitcherHandle() : base() { }
    public StackSwitcherHandle(nint obj) : base() => SetInternalHandle(obj);

    internal StackSwitcherHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class StackSwitcherHandleExtensions
{
    public static StackSwitcherHandle DownCastStackSwitcher(this WidgetHandle widget) => new(widget);
}

