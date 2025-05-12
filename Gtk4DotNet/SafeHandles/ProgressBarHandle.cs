namespace GtkDotNet.SafeHandles;

public class ProgressBarHandle : WidgetHandle
{
    public ProgressBarHandle() : base() { }
    public ProgressBarHandle(nint obj) : base() => SetInternalHandle(obj);

    internal ProgressBarHandle(WidgetHandle widget) : base() => handle = widget.TakeHandle();
}

public static class ProgressBarHandleExtensions
{
    public static ProgressBarHandle DownCastProgressBar(this WidgetHandle widget) => new(widget);
}

