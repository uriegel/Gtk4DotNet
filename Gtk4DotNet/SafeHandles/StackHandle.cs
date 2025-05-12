namespace GtkDotNet.SafeHandles;

public class StackHandle : WidgetHandle
{
    public StackHandle() : base() { }
    public StackHandle(nint obj) : base() => SetInternalHandle(obj);

    internal StackHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class StackHandleExtensions
{
    public static StackHandle DownCastStack(this WidgetHandle widget) => new(widget);
}


