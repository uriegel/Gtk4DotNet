namespace GtkDotNet.SafeHandles;

public class ButtonHandle : WidgetHandle
{
    public ButtonHandle() : base() { }
    public ButtonHandle(nint obj) : base() => SetInternalHandle(obj);
}
