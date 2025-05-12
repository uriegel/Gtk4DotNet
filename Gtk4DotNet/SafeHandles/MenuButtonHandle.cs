namespace GtkDotNet.SafeHandles;

public class MenuButtonHandle : WidgetHandle
{
    public MenuButtonHandle() : base() { }
    public MenuButtonHandle(nint obj) : base() => SetInternalHandle(obj);

    internal MenuButtonHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class MenuButtonHandleExtensions
{
    public static MenuButtonHandle DownCastMenuButton(this WidgetHandle widget) => new(widget);
}




