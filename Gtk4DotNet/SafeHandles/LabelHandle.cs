namespace GtkDotNet.SafeHandles;

public class LabelHandle : WidgetHandle
{
    public LabelHandle() : base() { }
    public LabelHandle(nint obj) : base() => SetInternalHandle(obj);
}
