namespace GtkDotNet.SafeHandles;

public class PopoverHandle : WidgetHandle
{
    public PopoverHandle() : base() { }
    public PopoverHandle(nint obj) : base() => SetInternalHandle(obj);
}
