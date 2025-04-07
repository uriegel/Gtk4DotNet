namespace GtkDotNet.SafeHandles;

public class AdwAlertDialogHandle : AdwDialogHandle
{
    public AdwAlertDialogHandle() : base() { }
    public AdwAlertDialogHandle(nint obj) : base() => SetInternalHandle(obj);
}
