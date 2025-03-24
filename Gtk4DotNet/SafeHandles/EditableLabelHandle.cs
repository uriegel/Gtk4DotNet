namespace GtkDotNet.SafeHandles;

public class EditableLabelHandle : WidgetHandle
{
    public EditableLabelHandle() : base() { }
    public EditableLabelHandle(nint obj) : base() => SetInternalHandle(obj);
}
