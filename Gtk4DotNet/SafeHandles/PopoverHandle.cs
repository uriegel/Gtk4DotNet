namespace GtkDotNet.SafeHandles;

public class PopoverHandle : WidgetHandle
{
    public PopoverHandle() : base() { }
    public PopoverHandle(nint obj) : base() => SetInternalHandle(obj);

    internal PopoverHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class PopoverHandleExtensions
{
    public static PopoverHandle DownCastPopover(this WidgetHandle widget) => new(widget);
}

