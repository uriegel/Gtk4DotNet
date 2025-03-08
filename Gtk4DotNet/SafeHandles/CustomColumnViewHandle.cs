using GtkDotNet.SubClassing;

namespace GtkDotNet.SafeHandles;

public class CustomColumnViewHandle : ScrolledWindowHandle
{
    public CustomColumnViewHandle() : base() { }
    public CustomColumnViewHandle(nint obj) : base() => SetInternalHandle(obj);
}
