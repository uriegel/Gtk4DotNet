using CsTools.Extensions;

namespace GtkDotNet.SafeHandles;

public class ListModelHandle : ObjectFloatingHandle, IListModel
{
    public ListModelHandle() : base() { }
    public ListModelHandle(nint obj) : base() => SetInternalHandle(obj);
}
