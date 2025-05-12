namespace GtkDotNet.SafeHandles;

public class MenuItemHandle : WidgetHandle
{
    public MenuItemHandle() : base() { }
    public MenuItemHandle(nint obj) : base() => SetInternalHandle(obj);

    internal MenuItemHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class MenuItemHandleExtensions
{
    public static MenuItemHandle DownCastMenuItem(this WidgetHandle widget) => new(widget);
}


