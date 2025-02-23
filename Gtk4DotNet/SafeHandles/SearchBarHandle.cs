namespace GtkDotNet.SafeHandles;

public class SearchBarHandle : WidgetHandle
{
    public SearchBarHandle() : base() { }
    public SearchBarHandle(nint obj) : base() => SetInternalHandle(obj);
}

