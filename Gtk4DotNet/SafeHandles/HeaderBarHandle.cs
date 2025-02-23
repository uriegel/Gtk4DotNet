namespace GtkDotNet.SafeHandles;

public class HeaderBarHandle : WidgetHandle
{
    public HeaderBarHandle() : base() { }
    public HeaderBarHandle(nint obj) : base() => SetInternalHandle(obj);
}
