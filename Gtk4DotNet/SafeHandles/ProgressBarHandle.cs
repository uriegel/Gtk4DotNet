namespace GtkDotNet.SafeHandles;

public class ProgressBarHandle : WidgetHandle
{
    public ProgressBarHandle() : base() { }
    public ProgressBarHandle(nint obj) : base() => SetInternalHandle(obj);
}
