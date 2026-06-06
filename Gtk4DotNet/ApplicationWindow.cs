namespace Gtk4DotNet;

public class ApplicationWindow : Window // , IActionMap
{
    public ApplicationWindow() : base() { }
    public ApplicationWindow(nint obj) : base() => SetInternalHandle(obj);

    internal ApplicationWindow(Widget widget) : base() => handle = widget.TakeHandle();
}


