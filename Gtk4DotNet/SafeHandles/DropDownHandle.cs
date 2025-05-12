namespace GtkDotNet.SafeHandles;

public class DropDownHandle : WidgetHandle
{
    public DropDownHandle() : base() { }
    public DropDownHandle(nint obj) : base() => SetInternalHandle(obj);
    internal DropDownHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class DropDownHandleExtensions
{
    public static DropDownHandle DownCastDropDown(this WidgetHandle widget) => new(widget);
}


