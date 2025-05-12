namespace GtkDotNet.SafeHandles;

public class EditableLabelHandle : WidgetHandle
{
    public EditableLabelHandle() : base() { }
    public EditableLabelHandle(nint obj) : base() => SetInternalHandle(obj);

    internal EditableLabelHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class EditableLabelHandleExtensions
{
    public static EditableLabelHandle DownCastEditableLabel(this WidgetHandle widget) => new(widget);
}

