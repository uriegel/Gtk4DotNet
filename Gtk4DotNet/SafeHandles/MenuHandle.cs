namespace GtkDotNet.SafeHandles;

public class MenuHandle : WidgetHandle
{
    public MenuHandle() : base() { }
    public MenuHandle(nint obj) : base() => SetInternalHandle(obj);

    internal MenuHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class MenuHandleExtensions
{
    public static MenuHandle DownCastMenu(this WidgetHandle widget) => new(widget);
}






