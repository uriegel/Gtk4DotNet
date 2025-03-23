namespace GtkDotNet.SafeHandles;

public class CheckButtonHandle : WidgetHandle
{
    public CheckButtonHandle() : base() { }
    public CheckButtonHandle(nint obj) : base() => SetInternalHandle(obj);
}
