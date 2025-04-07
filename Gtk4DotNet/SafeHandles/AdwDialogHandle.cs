namespace GtkDotNet.SafeHandles;

public class AdwDialogHandle : WidgetHandle
{
    public AdwDialogHandle() : base() { }
    public AdwDialogHandle(nint obj) : base() => SetInternalHandle(obj);
}
