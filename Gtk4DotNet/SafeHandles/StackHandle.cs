namespace GtkDotNet.SafeHandles;

public class StackHandle : WidgetHandle
{
    public StackHandle() : base() { }
    public StackHandle(nint obj) : base() => SetInternalHandle(obj);
}
